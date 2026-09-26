using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.Loader;

namespace ModularPipelines.TestHelpers;

public sealed class ModuleLoadContextBuilds : IDisposable
{
    private readonly AssemblyLoadContext _firstContext = new(null, isCollectible: true);
    private readonly AssemblyLoadContext _secondContext = new(null, isCollectible: true);

    public Assembly First { get; }

    public Assembly Second { get; }

    public ModuleLoadContextBuilds(Type moduleBaseType, bool identicalBuilds)
    {
        var name = $"ContextModule_{Guid.NewGuid():N}";
        var firstImage = BuildAssembly(name, "first", moduleBaseType);
        using var firstStream = new MemoryStream(firstImage);
        using var secondStream = new MemoryStream(identicalBuilds ? firstImage : BuildAssembly(name, "second", moduleBaseType));
        First = _firstContext.LoadFromStream(firstStream);
        Second = _secondContext.LoadFromStream(secondStream);
    }

    private static byte[] BuildAssembly(string name, string label, Type moduleBaseType)
    {
        var assembly = new PersistedAssemblyBuilder(new AssemblyName(name), typeof(object).Assembly);
        var module = assembly.DefineDynamicModule(name);
        var attributes = TypeAttributes.Public | (moduleBaseType.IsAbstract ? TypeAttributes.Abstract : 0);
        module.DefineType("ContextModule", attributes, moduleBaseType).CreateType();
        var value = module.DefineType("RuntimeValue", TypeAttributes.Public);
        var getter = value.DefineMethod("get_Label", MethodAttributes.Public | MethodAttributes.SpecialName, typeof(string), Type.EmptyTypes);
        var il = getter.GetILGenerator();
        il.Emit(OpCodes.Ldstr, label);
        il.Emit(OpCodes.Ret);
        value.DefineProperty("Label", PropertyAttributes.None, typeof(string), null).SetGetMethod(getter);
        value.CreateType();
        using var stream = new MemoryStream();
        assembly.Save(stream);
        return stream.ToArray();
    }

    public void Dispose()
    {
        _firstContext.Unload();
        _secondContext.Unload();
    }
}
