using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.Loader;

namespace ModularPipelines.TestHelpers;

public sealed class ModuleBaseBuilds : IDisposable
{
    private readonly AssemblyLoadContext _firstContext = new(null, isCollectible: true);
    private readonly AssemblyLoadContext _secondContext = new(null, isCollectible: true);

    public Type First { get; }

    public Type Second { get; }

    public ModuleBaseBuilds(Type moduleBase)
    {
        var name = $"ExternalBase_{Guid.NewGuid():N}";
        var firstBase = Load(_firstContext, BuildBase(name, moduleBase, "original")).GetType("ExternalBase")!;
        Load(_secondContext, BuildBase(name, moduleBase, "changed"));
        var leafAssembly = new PersistedAssemblyBuilder(new AssemblyName($"LeafModule_{Guid.NewGuid():N}"), typeof(object).Assembly);
        leafAssembly.DefineDynamicModule("Leaf").DefineType("LeafModule", TypeAttributes.Public, firstBase).CreateType();
        var image = Save(leafAssembly);
        First = Load(_firstContext, image).GetType("LeafModule")!;
        Second = Load(_secondContext, image).GetType("LeafModule")!;
    }

    private static byte[] BuildBase(string name, Type moduleBase, string label)
    {
        var assembly = new PersistedAssemblyBuilder(new AssemblyName(name), typeof(object).Assembly);
        var type = assembly.DefineDynamicModule(name).DefineType("ExternalBase", TypeAttributes.Public, moduleBase);
        var method = type.DefineMethod("GetLabel", MethodAttributes.Public | MethodAttributes.Virtual, typeof(string), Type.EmptyTypes);
        var il = method.GetILGenerator();
        il.Emit(OpCodes.Ldstr, label);
        il.Emit(OpCodes.Ret);
        type.DefineMethodOverride(method, moduleBase.GetMethod("GetLabel")!);
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
