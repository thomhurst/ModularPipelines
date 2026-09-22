using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.Loader;
using System.Text.Json;
using System.Text.Json.Serialization;
using ModularPipelines.Attributes;
using ModularPipelines.Distributed.Serialization;
using ModularPipelines.Enums;
using ModularPipelines.Modules;
using ModularPipelines.Serialization;

namespace ModularPipelines.Distributed.UnitTests.Serialization;

public class ResultBuildIdentityTests
{
    [ModuleId("inherited-result-build")]
    private abstract class ResultModule<T> : Module<T>;

    public class RecursiveBase<T>;

    public class RecursiveResult : RecursiveBase<RecursiveResult>;

    public class GenericRecursiveResult<T> : RecursiveBase<GenericRecursiveResult<T>>;

    public class RecursiveMemberResult
    {
        public RecursiveMemberResult? Next { get; set; }
    }

    public class ExpandingRecursiveResult<T>
    {
        public ExpandingRecursiveResult<ExpandingRecursiveResult<T>>? Next { get; set; }
    }

    [Test]
    public async Task Expanding_Generic_Member_Graph_Fingerprint_Terminates()
    {
        var fingerprint = StableTypeName.GetBuildFingerprint(typeof(ExpandingRecursiveResult<int>));
        await Assert.That(fingerprint).IsNotEmpty();
    }

    [Test]
    [Arguments("Property", 0)]
    [Arguments("Property", 1)]
    [Arguments("Field", 0)]
    [Arguments("TypeConverter", 0)]
    [Arguments("PropertyConverter", 0)]
    [Arguments("FieldConverter", 0)]
    [Arguments("Collection", 0)]
    [Arguments("Dictionary", 0)]
    public async Task Schema_And_Runtime_Reject_Changed_Member_With_Unchanged_Result_Binary(string memberKind, int intermediateLevels)
    {
        using var builds = new ResultBuilds(intermediateLevels, memberKind);
        var first = new ModuleTypeRegistry();
        var second = new ModuleTypeRegistry();
        first.Register(typeof(ResultModule<>).MakeGenericType(builds.First));
        second.Register(typeof(ResultModule<>).MakeGenericType(builds.Second));

        await Assert.That(builds.First.Module.ModuleVersionId).IsEqualTo(builds.Second.Module.ModuleVersionId);
        await Assert.That(first.GetPipelineSchemaVersion()).IsNotEqualTo(second.GetPipelineSchemaVersion());

        var localType = StableTypeName.Resolve(StableTypeName.Get(builds.First))!;
        var remoteType = localType == builds.First ? builds.Second : builds.First;
        var localJson = SerializeValue(localType);
        await Assert.That(JsonSerializer.Deserialize<ModuleResult<object>>(localJson)!.Value.GetType()).IsEqualTo(localType);

        var exception = Assert.Throws<JsonException>(() => JsonSerializer.Deserialize<ModuleResult<object>>(SerializeValue(remoteType)));
        await Assert.That(exception!.Message).Contains("build identity");
    }

    [Test]
    [Arguments(0)]
    [Arguments(1)]
    public async Task Schema_Rejects_Changed_Result_Base_With_Unchanged_Result_Binary(int intermediateLevels)
    {
        using var builds = new ResultBuilds(intermediateLevels);
        var first = new ModuleTypeRegistry();
        var second = new ModuleTypeRegistry();
        first.Register(typeof(ResultModule<>).MakeGenericType(builds.First));
        second.Register(typeof(ResultModule<>).MakeGenericType(builds.Second));

        await Assert.That(StableTypeName.Get(builds.First)).IsEqualTo(StableTypeName.Get(builds.Second));
        await Assert.That(builds.First.Module.ModuleVersionId).IsEqualTo(builds.Second.Module.ModuleVersionId);
        await Assert.That(first.GetPipelineSchemaVersion()).IsNotEqualTo(second.GetPipelineSchemaVersion());
    }

    [Test]
    [Arguments(0)]
    [Arguments(1)]
    public async Task Runtime_Result_Rejects_Changed_Base_With_Unchanged_Result_Binary(int intermediateLevels)
    {
        using var builds = new ResultBuilds(intermediateLevels);
        var localType = StableTypeName.Resolve(StableTypeName.Get(builds.First))!;
        var remoteType = localType == builds.First ? builds.Second : builds.First;
        var localJson = SerializeValue(localType);
        var localResult = JsonSerializer.Deserialize<ModuleResult<object>>(localJson);
        await Assert.That(localResult!.Value.GetType()).IsEqualTo(localType);

        var remoteJson = SerializeValue(remoteType);
        var exception = Assert.Throws<JsonException>(() => JsonSerializer.Deserialize<ModuleResult<object>>(remoteJson));
        await Assert.That(exception!.Message).Contains("build identity");
    }

    [Test]
    [Arguments(typeof(RecursiveResult))]
    [Arguments(typeof(GenericRecursiveResult<int>))]
    [Arguments(typeof(RecursiveMemberResult))]
    public async Task Recursive_Generic_Base_RoundTrips_Without_Fingerprint_Recursion(Type type)
    {
        var json = SerializeValue(type);
        var result = JsonSerializer.Deserialize<ModuleResult<object>>(json);
        await Assert.That(result!.Value.GetType()).IsEqualTo(type);
    }

    private static string SerializeValue(Type type) => JsonSerializer.Serialize(
        new ModuleResult<object>.Success(CreateValue(type))
        {
            Name = "InheritedResult",
            Duration = TimeSpan.Zero,
            StartTime = DateTimeOffset.UtcNow,
            EndTime = DateTimeOffset.UtcNow,
            Status = ModuleStatus.Succeeded,
        });

    private static object CreateValue(Type type)
    {
        var value = Activator.CreateInstance(type)!;
        var collection = type.GetInterfaces().FirstOrDefault(contract =>
            contract.IsGenericType && contract.GetGenericTypeDefinition() == typeof(ICollection<>));
        if (collection is not null)
        {
            var element = collection.GetGenericArguments()[0];
            var item = element.IsGenericType && element.GetGenericTypeDefinition() == typeof(KeyValuePair<,>)
                ? Activator.CreateInstance(element, "key", Activator.CreateInstance(element.GetGenericArguments()[1]))
                : Activator.CreateInstance(element);
            collection.GetMethod("Add")!.Invoke(value, [item]);
        }

        return value;
    }

    public class ObjectConverter : JsonConverter<object>
    {
        public override bool CanConvert(Type typeToConvert) => true;

        public override object Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using var document = JsonDocument.ParseValue(ref reader);
            return Activator.CreateInstance(typeToConvert)!;
        }

        public override void Write(Utf8JsonWriter writer, object value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteEndObject();
        }
    }

    private sealed class ResultBuilds : IDisposable
    {
        private readonly AssemblyLoadContext _firstContext = new(null, isCollectible: true);
        private readonly AssemblyLoadContext _secondContext = new(null, isCollectible: true);

        public Type First { get; }

        public Type Second { get; }

        public ResultBuilds(int intermediateLevels, string memberKind = "Base")
        {
            var baseName = $"ResultBase_{Guid.NewGuid():N}";
            var converter = memberKind.EndsWith("Converter", StringComparison.Ordinal);
            var firstBase = Load(_firstContext, BuildBase(baseName, "Original", converter)).GetType("ResultBase")!;
            Load(_secondContext, BuildBase(baseName, "Changed", converter));

            var derived = new PersistedAssemblyBuilder(new AssemblyName($"Derived_{Guid.NewGuid():N}"), typeof(object).Assembly);
            var module = derived.DefineDynamicModule("Derived");
            var parent = firstBase;
            for (var level = 0; level < intermediateLevels; level++)
            {
                parent = DefineResultType(module, $"Intermediate{level}", parent, memberKind);
            }

            DefineResultType(module, "Result", parent, memberKind);
            var image = Save(derived);
            First = Load(_firstContext, image).GetType("Result")!;
            Second = Load(_secondContext, image).GetType("Result")!;
        }

        private static Type DefineResultType(ModuleBuilder module, string name, Type dependency, string memberKind)
        {
            if (memberKind is "Collection" or "Dictionary")
            {
                return DefineCollectionType(module, name, dependency, memberKind == "Dictionary");
            }

            var type = module.DefineType(name, TypeAttributes.Public, memberKind == "Base" ? dependency : typeof(object));
            CustomAttributeBuilder ConverterAttribute() => new(
                typeof(JsonConverterAttribute).GetConstructor([typeof(Type)])!, [dependency]);
            switch (memberKind)
            {
                case "TypeConverter":
                    type.SetCustomAttribute(ConverterAttribute());
                    break;
                case "Property":
                case "PropertyConverter":
                    var propertyType = memberKind == "PropertyConverter" ? typeof(object) : dependency;
                    var property = type.DefineProperty("Value", PropertyAttributes.None, propertyType, null);
                    if (memberKind == "PropertyConverter")
                    {
                        property.SetCustomAttribute(ConverterAttribute());
                    }

                    var getter = type.DefineMethod("get_Value",
                        MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig, propertyType, Type.EmptyTypes);
                    var il = getter.GetILGenerator();
                    il.Emit(OpCodes.Ldnull);
                    il.Emit(OpCodes.Ret);
                    property.SetGetMethod(getter);
                    break;
                case "Field":
                case "FieldConverter":
                    var fieldType = memberKind == "FieldConverter" ? typeof(object) : dependency;
                    var field = type.DefineField("Value", fieldType, FieldAttributes.Public);
                    field.SetCustomAttribute(new CustomAttributeBuilder(
                        typeof(JsonIncludeAttribute).GetConstructor(Type.EmptyTypes)!, []));
                    if (memberKind == "FieldConverter")
                    {
                        field.SetCustomAttribute(ConverterAttribute());
                    }

                    break;
            }

            return type.CreateType()!;
        }

        private static Type DefineCollectionType(ModuleBuilder module, string name, Type element, bool dictionary)
        {
            var storage = dictionary ? typeof(Dictionary<,>).MakeGenericType(typeof(string), element) : typeof(List<>).MakeGenericType(element);
            var contract = dictionary ? typeof(IDictionary<,>).MakeGenericType(typeof(string), element) : typeof(ICollection<>).MakeGenericType(element);
            var type = module.DefineType(name, TypeAttributes.Public, typeof(object));
            var field = type.DefineField("_items", storage, FieldAttributes.Private);
            var constructor = type.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, Type.EmptyTypes).GetILGenerator();
            constructor.Emit(OpCodes.Ldarg_0);
            constructor.Emit(OpCodes.Call, typeof(object).GetConstructor(Type.EmptyTypes)!);
            constructor.Emit(OpCodes.Ldarg_0);
            constructor.Emit(OpCodes.Newobj, storage.GetConstructor(Type.EmptyTypes)!);
            constructor.Emit(OpCodes.Stfld, field);
            constructor.Emit(OpCodes.Ret);

            // Only explicit interface members expose the element type. Public member/base
            // reflection cannot discover the contract used by System.Text.Json here.
            var index = 0;
            foreach (var implemented in contract.GetInterfaces().Append(contract))
            {
                type.AddInterfaceImplementation(implemented);
                foreach (var method in implemented.GetMethods())
                {
                    var parameters = method.GetParameters().Select(parameter => parameter.ParameterType).ToArray();
                    var forwarding = type.DefineMethod($"Forward{index++}",
                        MethodAttributes.Private | MethodAttributes.Virtual | MethodAttributes.Final | MethodAttributes.NewSlot,
                        method.ReturnType, parameters);
                    var il = forwarding.GetILGenerator();
                    il.Emit(OpCodes.Ldarg_0);
                    il.Emit(OpCodes.Ldfld, field);
                    for (var argument = 0; argument < parameters.Length; argument++)
                    {
                        il.Emit(OpCodes.Ldarg, argument + 1);
                    }

                    il.Emit(OpCodes.Callvirt, method);
                    il.Emit(OpCodes.Ret);
                    type.DefineMethodOverride(forwarding, method);
                }
            }

            return type.CreateType()!;
        }

        private static byte[] BuildBase(string name, string propertyName, bool converter)
        {
            var assembly = new PersistedAssemblyBuilder(new AssemblyName(name), typeof(object).Assembly);
            var type = assembly.DefineDynamicModule(name).DefineType("ResultBase", TypeAttributes.Public,
                converter ? typeof(ObjectConverter) : typeof(object));
            var property = type.DefineProperty(propertyName, PropertyAttributes.None, typeof(string), null);
            var getter = type.DefineMethod($"get_{propertyName}",
                MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig, typeof(string), Type.EmptyTypes);
            var il = getter.GetILGenerator();
            il.Emit(OpCodes.Ldstr, propertyName);
            il.Emit(OpCodes.Ret);
            property.SetGetMethod(getter);
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
}
