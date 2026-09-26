using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.Loader;

namespace ModularPipelines.TestHelpers;

public sealed class InterfaceMemberBuilds : IDisposable
{
    private readonly AssemblyLoadContext _firstContext = new(null, isCollectible: true);
    private readonly AssemblyLoadContext _secondContext = new(null, isCollectible: true);

    public Type First { get; }

    public Type Second { get; }

    public InterfaceMemberBuilds(Type moduleBaseType, bool inherited, bool typeConstraint = false, bool moduleConstraint = false, string? constraintDependency = null)
    {
        var dependencyName = $"InterfaceMember_{Guid.NewGuid():N}";
        var interfaceDependency = (typeConstraint || moduleConstraint) && constraintDependency != "Member";
        var dependency = Load(_firstContext, BuildDependency(dependencyName, "original", interfaceDependency)).GetType("MemberValue")!;
        Load(_secondContext, BuildDependency(dependencyName, "changed", interfaceDependency));
        var assembly = new PersistedAssemblyBuilder(new AssemblyName($"InterfaceModule_{Guid.NewGuid():N}"), typeof(object).Assembly);
        var module = assembly.DefineDynamicModule("InterfaceModule");
        var constraint = constraintDependency is null ? dependency : DefineIndirectConstraint(module, dependency, constraintDependency);
        var labelContract = constraintDependency == "Inherited" ? dependency : constraint;
        var valueType = typeConstraint || moduleConstraint ? DefineConstrainedValue(module, constraint, labelContract) : dependency;
        if (moduleConstraint)
        {
            DefineConstrainedModule(module, moduleBaseType, constraint, valueType, inherited);
        }
        else
        {
            DefineInterfaceModule(module, moduleBaseType, constraint, valueType, inherited, typeConstraint);
        }

        var image = Save(assembly);
        First = ResolveModule(Load(_firstContext, image));
        Second = ResolveModule(Load(_secondContext, image));
    }

    private static Type ResolveModule(Assembly assembly)
    {
        var module = assembly.GetType("ContractModule")!;
        return module.IsGenericTypeDefinition ? module.MakeGenericType(assembly.GetType("Payload")!) : module;
    }

    private static void DefineConstrainedModule(ModuleBuilder module, Type moduleBaseType, Type dependency, Type valueType, bool inherited)
    {
        var implementation = module.DefineType(inherited ? "GenericModule" : "ContractModule", TypeAttributes.Public, moduleBaseType);
        implementation.DefineGenericParameters("T")[0].SetInterfaceConstraints(dependency);
        var constructor = implementation.DefineDefaultConstructor(MethodAttributes.Public);
        DefineValueGetter(implementation, valueType);
        var closed = implementation.CreateType()!.MakeGenericType(valueType);
        if (inherited)
        {
            var derived = module.DefineType("ContractModule", TypeAttributes.Public, closed);
            var il = derived.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, Type.EmptyTypes).GetILGenerator();
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Call, TypeBuilder.GetConstructor(closed, constructor));
            il.Emit(OpCodes.Ret);
            derived.CreateType();
        }
    }

    private static void DefineInterfaceModule(ModuleBuilder module, Type moduleBaseType, Type dependency, Type valueType, bool inherited, bool typeConstraint)
    {
        var contract = module.DefineType("IContract", TypeAttributes.Public | TypeAttributes.Interface | TypeAttributes.Abstract);
        if (typeConstraint)
        {
            contract.DefineGenericParameters("T")[0].SetInterfaceConstraints(dependency);
        }

        var getter = contract.DefineMethod("get_Value", MethodAttributes.Public | MethodAttributes.Abstract | MethodAttributes.Virtual
            | MethodAttributes.NewSlot | MethodAttributes.SpecialName, valueType, Type.EmptyTypes);
        contract.DefineProperty("Value", PropertyAttributes.None, valueType, null).SetGetMethod(getter);
        var declaredContract = contract.CreateType()!;
        if (typeConstraint)
        {
            declaredContract = declaredContract.MakeGenericType(valueType);
        }

        var implementedContract = declaredContract;
        if (inherited)
        {
            var derived = module.DefineType("IDerivedContract", TypeAttributes.Public | TypeAttributes.Interface | TypeAttributes.Abstract);
            derived.AddInterfaceImplementation(declaredContract);
            implementedContract = derived.CreateType()!;
        }

        var implementation = module.DefineType("ContractModule", TypeAttributes.Public, moduleBaseType);
        implementation.AddInterfaceImplementation(implementedContract);
        var implementationGetter = DefineValueGetter(implementation, valueType);
        implementation.DefineMethodOverride(implementationGetter, typeConstraint ? TypeBuilder.GetMethod(declaredContract, getter) : getter);
        implementation.CreateType();
    }

    private static MethodBuilder DefineValueGetter(TypeBuilder implementation, Type valueType)
    {
        var getter = implementation.DefineMethod("get_Value", MethodAttributes.Public | MethodAttributes.Virtual
            | MethodAttributes.Final | MethodAttributes.NewSlot | MethodAttributes.SpecialName, valueType, Type.EmptyTypes);
        var il = getter.GetILGenerator();
        il.Emit(OpCodes.Newobj, valueType.GetConstructor(Type.EmptyTypes)!);
        il.Emit(OpCodes.Ret);
        implementation.DefineProperty("Value", PropertyAttributes.None, valueType, null).SetGetMethod(getter);
        return getter;
    }

    private static Type DefineIndirectConstraint(ModuleBuilder module, Type dependency, string kind)
    {
        var type = module.DefineType("IConstraint", TypeAttributes.Public | TypeAttributes.Interface | TypeAttributes.Abstract);
        if (kind == "Inherited")
        {
            type.AddInterfaceImplementation(dependency);
            return type.CreateType()!;
        }

        const MethodAttributes attributes = MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.NewSlot | MethodAttributes.SpecialName;
        var valueGetter = type.DefineMethod("get_Value", attributes, dependency, Type.EmptyTypes);
        var valueIl = valueGetter.GetILGenerator();
        valueIl.Emit(OpCodes.Newobj, dependency.GetConstructor(Type.EmptyTypes)!);
        valueIl.Emit(OpCodes.Ret);
        type.DefineProperty("Value", PropertyAttributes.None, dependency, null).SetGetMethod(valueGetter);
        var labelGetter = type.DefineMethod("get_Label", attributes, typeof(string), Type.EmptyTypes);
        var labelIl = labelGetter.GetILGenerator();
        labelIl.Emit(OpCodes.Ldarg_0);
        labelIl.Emit(OpCodes.Callvirt, valueGetter);
        labelIl.Emit(OpCodes.Callvirt, dependency.GetMethod("get_Label")!);
        labelIl.Emit(OpCodes.Ret);
        type.DefineProperty("Label", PropertyAttributes.None, typeof(string), null).SetGetMethod(labelGetter);
        return type.CreateType()!;
    }

    private static Type DefineConstrainedValue(ModuleBuilder module, Type dependency, Type labelContract)
    {
        // The payload binary stays identical and exposes no external member types.
        // Its label calls the independently rebuilt default interface implementation.
        var type = module.DefineType("Payload", TypeAttributes.Public);
        type.AddInterfaceImplementation(dependency);
        var getter = type.DefineMethod("get_Label", MethodAttributes.Public | MethodAttributes.SpecialName, typeof(string), Type.EmptyTypes);
        var il = getter.GetILGenerator();
        il.Emit(OpCodes.Ldarg_0);
        il.Emit(OpCodes.Callvirt, labelContract.GetMethod("get_Label")!);
        il.Emit(OpCodes.Ret);
        type.DefineProperty("Label", PropertyAttributes.None, typeof(string), null).SetGetMethod(getter);
        return type.CreateType()!;
    }

    private static byte[] BuildDependency(string name, string label, bool typeConstraint)
    {
        var assembly = new PersistedAssemblyBuilder(new AssemblyName(name), typeof(object).Assembly);
        var attributes = typeConstraint ? TypeAttributes.Public | TypeAttributes.Interface | TypeAttributes.Abstract : TypeAttributes.Public;
        var type = assembly.DefineDynamicModule(name).DefineType("MemberValue", attributes);
        var getter = type.DefineMethod("get_Label", MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.Virtual | MethodAttributes.NewSlot,
            typeof(string), Type.EmptyTypes);
        var il = getter.GetILGenerator();
        il.Emit(OpCodes.Ldstr, label);
        il.Emit(OpCodes.Ret);
        type.DefineProperty("Label", PropertyAttributes.None, typeof(string), null).SetGetMethod(getter);
        type.CreateType();
        return Save(assembly);
    }

    private static byte[] Save(PersistedAssemblyBuilder assembly)
    {
        using var stream = new MemoryStream();
        assembly.Save(stream);
        return stream.ToArray();
    }

    private static Assembly Load(AssemblyLoadContext context, byte[] image)
    {
        using var stream = new MemoryStream(image);
        return context.LoadFromStream(stream);
    }

    public void Dispose()
    {
        _firstContext.Unload();
        _secondContext.Unload();
    }
}
