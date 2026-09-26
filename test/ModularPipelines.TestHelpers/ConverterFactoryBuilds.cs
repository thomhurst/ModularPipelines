using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.Loader;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ModularPipelines.TestHelpers;

public sealed class ConverterFactoryBuilds : IDisposable
{
    private readonly AssemblyLoadContext _firstContext = new(null, isCollectible: true);
    private readonly AssemblyLoadContext _secondContext = new(null, isCollectible: true);

    public Type First { get; }

    public Type Second { get; }

    public ConverterFactoryBuilds(bool memberConverter)
    {
        var name = $"OptionConverter_{Guid.NewGuid():N}";
        var converter = Load(_firstContext, BuildConverter(name, "original")).GetType("Converter")!;
        Load(_secondContext, BuildConverter(name, "changed"));
        var assembly = new PersistedAssemblyBuilder(new AssemblyName($"OptionValue_{Guid.NewGuid():N}"), typeof(object).Assembly);
        var module = assembly.DefineDynamicModule("Value");
        var value = module.DefineType("Value", TypeAttributes.Public);
        var converterGetter = value.DefineMethod("get_ConverterType", MethodAttributes.Public | MethodAttributes.Static, typeof(Type), Type.EmptyTypes);
        var il = converterGetter.GetILGenerator();
        il.Emit(OpCodes.Ldtoken, converter);
        il.Emit(OpCodes.Call, typeof(Type).GetMethod(nameof(Type.GetTypeFromHandle))!);
        il.Emit(OpCodes.Ret);
        value.DefineProperty("ConverterType", PropertyAttributes.None, typeof(Type), null).SetGetMethod(converterGetter);
        var attribute = new CustomAttributeBuilder(typeof(JsonConverterAttribute).GetConstructor([typeof(Type)])!, [typeof(OptionsFactory)]);
        if (!memberConverter)
        {
            value.SetCustomAttribute(attribute);
        }

        var valueType = value.CreateType()!;
        if (memberConverter)
        {
            var wrapper = module.DefineType("Wrapper", TypeAttributes.Public);
            var getter = wrapper.DefineMethod("get_Value", MethodAttributes.Public, valueType, Type.EmptyTypes);
            il = getter.GetILGenerator();
            il.Emit(OpCodes.Newobj, valueType.GetConstructor(Type.EmptyTypes)!);
            il.Emit(OpCodes.Ret);
            var property = wrapper.DefineProperty("Value", PropertyAttributes.None, valueType, null);
            property.SetGetMethod(getter);
            property.SetCustomAttribute(attribute);
            wrapper.CreateType();
        }

        var image = Save(assembly);
        var resultName = memberConverter ? "Wrapper" : "Value";
        First = Load(_firstContext, image).GetType(resultName)!;
        Second = Load(_secondContext, image).GetType(resultName)!;
    }

    public sealed class OptionsFactory : JsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert) => true;

        public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options) =>
            !options.IsReadOnly || options.PropertyNameCaseInsensitive
                ? new ValueConverter()
                : (JsonConverter) Activator.CreateInstance((Type) typeToConvert.GetProperty("ConverterType")!.GetValue(null)!)!;
    }

    public class ValueConverter : JsonConverter<object>
    {
        public virtual string GetLabel() => "distributed";

        public override object Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
            Activator.CreateInstance(typeToConvert)!;

        public override void Write(Utf8JsonWriter writer, object value, JsonSerializerOptions options) => writer.WriteStringValue(GetLabel());
    }

    private static byte[] BuildConverter(string name, string label)
    {
        var assembly = new PersistedAssemblyBuilder(new AssemblyName(name), typeof(object).Assembly);
        var type = assembly.DefineDynamicModule(name).DefineType("Converter", TypeAttributes.Public, typeof(ValueConverter));
        var method = type.DefineMethod(nameof(ValueConverter.GetLabel), MethodAttributes.Public | MethodAttributes.Virtual, typeof(string), Type.EmptyTypes);
        var il = method.GetILGenerator();
        il.Emit(OpCodes.Ldstr, label);
        il.Emit(OpCodes.Ret);
        type.DefineMethodOverride(method, typeof(ValueConverter).GetMethod(nameof(ValueConverter.GetLabel))!);
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
