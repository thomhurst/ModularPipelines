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

    public InterfaceMemberBuilds(Type moduleBaseType, bool inherited, bool typeConstraint = false)
    {
        var dependencyName = $"InterfaceMember_{Guid.NewGuid():N}";
        var dependency = Load(_firstContext, BuildDependency(dependencyName, "original", typeConstraint)).GetType("MemberValue")!;
        Load(_secondContext, BuildDependency(dependencyName, "changed", typeConstraint));
        var assembly = new PersistedAssemblyBuilder(new AssemblyName($"InterfaceModule_{Guid.NewGuid():N}"), typeof(object).Assembly);
        var module = assembly.DefineDynamicModule("InterfaceModule");
        var valueType = typeConstraint ? DefineConstrainedValue(module, dependency) : dependency;
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
        var implementationGetter = implementation.DefineMethod("get_Value", MethodAttributes.Public | MethodAttributes.Virtual
            | MethodAttributes.Final | MethodAttributes.NewSlot | MethodAttributes.SpecialName, valueType, Type.EmptyTypes);
        var il = implementationGetter.GetILGenerator();
        il.Emit(OpCodes.Newobj, valueType.GetConstructor(Type.EmptyTypes)!);
        il.Emit(OpCodes.Ret);
        implementation.DefineProperty("Value", PropertyAttributes.None, valueType, null).SetGetMethod(implementationGetter);
        implementation.DefineMethodOverride(implementationGetter, typeConstraint ? TypeBuilder.GetMethod(declaredContract, getter) : getter);
        implementation.CreateType();
        var image = Save(assembly);
        First = Load(_firstContext, image).GetType("ContractModule")!;
        Second = Load(_secondContext, image).GetType("ContractModule")!;
    }

    private static Type DefineConstrainedValue(ModuleBuilder module, Type dependency)
    {
        // The payload binary stays identical and exposes no external member types.
        // Its label calls the independently rebuilt default interface implementation.
        var type = module.DefineType("Payload", TypeAttributes.Public);
        type.AddInterfaceImplementation(dependency);
        var getter = type.DefineMethod("get_Label", MethodAttributes.Public | MethodAttributes.SpecialName, typeof(string), Type.EmptyTypes);
        var il = getter.GetILGenerator();
        il.Emit(OpCodes.Ldarg_0);
        il.Emit(OpCodes.Callvirt, dependency.GetMethod("get_Label")!);
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
