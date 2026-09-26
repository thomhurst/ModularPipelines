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
    [Arguments("IncludedPrivateGetter", 0)]
    [Arguments("IncludedOverride", 0)]
    [Arguments("IgnoredOverrideWithBaseMember", 0)]
    [Arguments("OpaquePropertyWithVisibleMember", 0)]
    [Arguments("OpaqueWrappedPropertyWithVisibleMember", 0)]
    [Arguments("OpaqueForwardingProperty", 0)]
    [Arguments("OpaqueForwardingField", 0)]
    [Arguments("Field", 0)]
    [Arguments("TypeConverter", 0)]
    [Arguments("PropertyConverter", 0)]
    [Arguments("FieldConverter", 0)]
    [Arguments("TypeFactory", 0)]
    [Arguments("PropertyFactory", 0)]
    [Arguments("FieldFactory", 0)]
    [Arguments("TypeAttribute", 0)]
    [Arguments("PropertyAttribute", 0)]
    [Arguments("FieldAttribute", 0)]
    [Arguments("TypeFactoryAttribute", 0)]
    [Arguments("PropertyFactoryAttribute", 0)]
    [Arguments("FieldFactoryAttribute", 0)]
    [Arguments("Collection", 0)]
    [Arguments("Dictionary", 0)]
    public async Task Schema_And_Runtime_Reject_Changed_Member_With_Unchanged_Result_Binary(string memberKind, int intermediateLevels)
    {
        using var builds = new ResultBuilds(intermediateLevels, memberKind);
        var first = new ModuleTypeRegistry();
        var second = new ModuleTypeRegistry();
        first.Register(typeof(ResultModule<>).MakeGenericType(builds.First));
        second.Register(typeof(ResultModule<>).MakeGenericType(builds.Second));

        if (memberKind.Contains("Forwarding", StringComparison.Ordinal))
        {
            await Assert.That(JsonSerializer.Serialize(CreateValue(builds.First)))
                .IsNotEqualTo(JsonSerializer.Serialize(CreateValue(builds.Second)));
        }

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
    [Arguments("UnserializedField")]
    [Arguments("PrivateGetter")]
    [Arguments("SetterOnly")]
    [Arguments("IncludedSetterOnly")]
    [Arguments("IgnoredOverride")]
    public async Task Unserialized_Member_Build_Does_Not_Change_Schema_Or_Reject_Result(string memberKind)
    {
        using var builds = new ResultBuilds(0, memberKind);
        var first = new ModuleTypeRegistry();
        var second = new ModuleTypeRegistry();
        first.Register(typeof(ResultModule<>).MakeGenericType(builds.First));
        second.Register(typeof(ResultModule<>).MakeGenericType(builds.Second));

        await Assert.That(builds.First.Module.ModuleVersionId).IsEqualTo(builds.Second.Module.ModuleVersionId);
        var firstMemberType = builds.First.GetField("Value")?.FieldType ?? builds.First.GetProperty("Value")!.PropertyType;
        var secondMemberType = builds.Second.GetField("Value")?.FieldType ?? builds.Second.GetProperty("Value")!.PropertyType;
        await Assert.That(firstMemberType.Module.ModuleVersionId).IsNotEqualTo(secondMemberType.Module.ModuleVersionId);
        await Assert.That(JsonSerializer.Serialize(CreateValue(builds.First))).IsEqualTo("{}");
        await Assert.That(JsonSerializer.Serialize(CreateValue(builds.Second))).IsEqualTo("{}");
        await Assert.That(first.GetPipelineSchemaVersion()).IsEqualTo(second.GetPipelineSchemaVersion());

        var localType = StableTypeName.Resolve(StableTypeName.Get(builds.First))!;
        var remoteType = localType == builds.First ? builds.Second : builds.First;
        var result = JsonSerializer.Deserialize<ModuleResult<object>>(SerializeValue(remoteType));
        await Assert.That(result!.Value.GetType()).IsEqualTo(localType);
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
    [Arguments("Property", JsonIgnoreCondition.Always, false)]
    [Arguments("Field", JsonIgnoreCondition.Always, false)]
    [Arguments("Property", JsonIgnoreCondition.WhenWriting, false)]
    [Arguments("Field", JsonIgnoreCondition.WhenWriting, false)]
    [Arguments("Property", JsonIgnoreCondition.WhenReading, true)]
    [Arguments("Field", JsonIgnoreCondition.WhenReading, true)]
    [Arguments("Property", JsonIgnoreCondition.Never, true)]
    [Arguments("Field", JsonIgnoreCondition.Never, true)]
    [Arguments("Property", JsonIgnoreCondition.WhenWritingNull, true)]
    [Arguments("Field", JsonIgnoreCondition.WhenWritingNull, true)]
    [Arguments("Property", JsonIgnoreCondition.WhenWritingDefault, true)]
    [Arguments("Field", JsonIgnoreCondition.WhenWritingDefault, true)]
    public async Task Ignore_Condition_Controls_Member_Build_Validation(string memberKind, JsonIgnoreCondition condition, bool canBeWritten)
    {
        using var builds = new ResultBuilds(0, memberKind, condition);
        var first = new ModuleTypeRegistry();
        var second = new ModuleTypeRegistry();
        first.Register(typeof(ResultModule<>).MakeGenericType(builds.First));
        second.Register(typeof(ResultModule<>).MakeGenericType(builds.Second));
        await Assert.That(builds.First.Module.ModuleVersionId).IsEqualTo(builds.Second.Module.ModuleVersionId);
        await Assert.That(first.GetPipelineSchemaVersion() != second.GetPipelineSchemaVersion()).IsEqualTo(canBeWritten);

        var localType = StableTypeName.Resolve(StableTypeName.Get(builds.First))!;
        var remoteType = localType == builds.First ? builds.Second : builds.First;
        if (canBeWritten)
        {
            var exception = Assert.Throws<JsonException>(() => JsonSerializer.Deserialize<ModuleResult<object>>(SerializeValue(remoteType)));
            await Assert.That(exception!.Message).Contains("build identity");
        }
        else
        {
            await Assert.That(JsonSerializer.Serialize(CreateValue(builds.First))).IsEqualTo("{}");
            await Assert.That(JsonSerializer.Serialize(CreateValue(builds.Second))).IsEqualTo("{}");
            var result = JsonSerializer.Deserialize<ModuleResult<object>>(SerializeValue(remoteType));
            await Assert.That(result!.Value.GetType()).IsEqualTo(localType);
        }
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

    [Test]
    [Arguments("ShadowProperty", null, false, "{\"Value\":null}")]
    [Arguments("ShadowProperty", JsonIgnoreCondition.Always, true, "{\"Value\":null}")]
    [Arguments("ShadowProperty", JsonIgnoreCondition.WhenWriting, false, "{}")]
    [Arguments("ShadowRenamed", null, true, "{\"Renamed\":null,\"Value\":null}")]
    [Arguments("ShadowSetterOnly", null, false, "{}")]
    [Arguments("ShadowPrivateGetter", null, false, "{}")]
    [Arguments("ShadowIncludedPrivateGetter", null, false, "{\"Value\":null}")]
    [Arguments("ShadowField", null, false, "{\"Value\":null}")]
    [Arguments("ShadowBaseField", null, false, "{\"Value\":null}")]
    [Arguments("OpaqueType", null, false, "{}")]
    [Arguments("OpaqueProperty", null, true, "{\"Value\":{}}")]
    [Arguments("OpaqueField", null, true, "{\"Value\":{}}")]
    [Arguments("OpaqueWrappedProperty", null, false, "{\"Value\":{}}")]
    [Arguments("OpaqueWrappedField", null, false, "{\"Value\":{}}")]
    [Arguments("OpaqueGenericProperty", null, true, "{\"Value\":{}}")]
    [Arguments("OpaqueInheritedProperty", null, true, "{\"Value\":{}}")]
    [Arguments("BaseTypeConverter", null, false, "{}")]
    [Arguments("BaseTypeFactory", null, false, "{}")]
    [Arguments("BaseTypeAttribute", null, false, "{}")]
    [Arguments("BaseTypeFactoryAttribute", null, false, "{}")]
    [Arguments("ListPropertyCollectionMember", null, false, "[0]")]
    [Arguments("ListFieldCollectionMember", null, false, "[0]")]
    [Arguments("DictionaryPropertyCollectionMember", null, false, "{\"key\":0}")]
    [Arguments("DictionaryFieldCollectionMember", null, false, "{\"key\":0}")]
    [Arguments("ArrayListPropertyCollectionMember", null, false, "[]")]
    [Arguments("ArrayListFieldCollectionMember", null, false, "[]")]
    public async Task Effective_Member_Build_Follows_Serialized_Contract(string memberKind, JsonIgnoreCondition? condition, bool includesDependency, string expectedJson)
    {
        using var builds = new ResultBuilds(0, memberKind, condition);
        var first = new ModuleTypeRegistry();
        var second = new ModuleTypeRegistry();
        first.Register(typeof(ResultModule<>).MakeGenericType(builds.First));
        second.Register(typeof(ResultModule<>).MakeGenericType(builds.Second));

        await Assert.That(builds.First.Module.ModuleVersionId).IsEqualTo(builds.Second.Module.ModuleVersionId);
        await Assert.That(JsonSerializer.Serialize(CreateValue(builds.First))).IsEqualTo(expectedJson);
        await Assert.That(JsonSerializer.Serialize(CreateValue(builds.Second))).IsEqualTo(expectedJson);
        await Assert.That(first.GetPipelineSchemaVersion() != second.GetPipelineSchemaVersion()).IsEqualTo(includesDependency);

        var localType = StableTypeName.Resolve(StableTypeName.Get(builds.First))!;
        var remoteType = localType == builds.First ? builds.Second : builds.First;
        if (includesDependency)
        {
            var exception = Assert.Throws<JsonException>(() => JsonSerializer.Deserialize<ModuleResult<object>>(SerializeValue(remoteType)));
            await Assert.That(exception!.Message).Contains("build identity");
        }
        else
        {
            var result = JsonSerializer.Deserialize<ModuleResult<object>>(SerializeValue(remoteType));
            await Assert.That(result!.Value.GetType()).IsEqualTo(localType);
        }
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

    public sealed class NullableConverterResult
    {
        [JsonConverter(typeof(UnderlyingIntFactory))]
        public int? Value { get; set; }
    }

    public sealed class ForwardingObjectConverter : ObjectConverter
    {
        public override void Write(Utf8JsonWriter writer, object value, JsonSerializerOptions options) =>
            JsonSerializer.Serialize(writer, value, value.GetType(), options);
    }

    public sealed class UnderlyingIntFactory : JsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert) => typeToConvert == typeof(int);

        public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options) =>
            typeToConvert == typeof(int) ? new IntConverter() : throw new InvalidOperationException("Expected the underlying value type.");
    }

    public sealed class IntConverter : JsonConverter<int>
    {
        public override int Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => reader.GetInt32();

        public override void Write(Utf8JsonWriter writer, int value, JsonSerializerOptions options) => writer.WriteNumberValue(value);
    }

    [Test]
    public async Task Nullable_Member_Factory_Uses_Underlying_Type()
    {
        await Assert.That(StableTypeName.GetBuildFingerprint(typeof(NullableConverterResult))).IsNotEmpty();
        var json = JsonSerializer.Serialize(new NullableConverterResult { Value = 42 });
        await Assert.That(JsonSerializer.Deserialize<NullableConverterResult>(json)!.Value).IsEqualTo(42);
    }

    private sealed class ResultBuilds : IDisposable
    {
        private readonly AssemblyLoadContext _firstContext = new(null, isCollectible: true);
        private readonly AssemblyLoadContext _secondContext = new(null, isCollectible: true);

        public Type First { get; }

        public Type Second { get; }

        public ResultBuilds(int intermediateLevels, string memberKind = "Base", JsonIgnoreCondition? ignoreCondition = null)
        {
            var baseName = $"ResultBase_{Guid.NewGuid():N}";
            var factory = memberKind.EndsWith("Factory", StringComparison.Ordinal);
            var attribute = memberKind.EndsWith("Attribute", StringComparison.Ordinal);
            var converter = factory || attribute || memberKind.EndsWith("Converter", StringComparison.Ordinal);
            var firstBase = Load(_firstContext, BuildBase(baseName, "Original", converter)).GetType("ResultBase")!;
            Load(_secondContext, BuildBase(baseName, "Changed", converter));
            if (memberKind.EndsWith("FactoryAttribute", StringComparison.Ordinal))
            {
                var innerFactoryImage = BuildConverterFactory(firstBase, attribute: false);
                firstBase = Load(_firstContext, innerFactoryImage).GetType("ConverterFactory")!;
                Load(_secondContext, innerFactoryImage);
                memberKind = memberKind.Replace("FactoryAttribute", "Attribute", StringComparison.Ordinal);
            }
            if (factory || attribute)
            {
                var factoryImage = BuildConverterFactory(firstBase, attribute);
                firstBase = Load(_firstContext, factoryImage).GetType("ConverterFactory")!;
                Load(_secondContext, factoryImage);
                memberKind = memberKind.Replace(factory ? "Factory" : "Attribute", "Converter", StringComparison.Ordinal);
            }

            var derived = new PersistedAssemblyBuilder(new AssemblyName($"Derived_{Guid.NewGuid():N}"), typeof(object).Assembly);
            var module = derived.DefineDynamicModule("Derived");
            var parent = firstBase;
            if (memberKind.EndsWith("CollectionMember", StringComparison.Ordinal))
            {
                parent = DefineCollectionMemberType(module, parent, memberKind);
                memberKind = "Base";
            }

            if (memberKind == "BaseTypeConverter")
            {
                parent = DefineResultType(module, "ConvertedBase", parent, "TypeConverter", null);
                memberKind = "Base";
            }

            for (var level = 0; level < intermediateLevels; level++)
            {
                parent = DefineResultType(module, $"Intermediate{level}", parent, memberKind, ignoreCondition);
            }

            DefineResultType(module, "Result", parent, memberKind, ignoreCondition);
            var image = Save(derived);
            First = Load(_firstContext, image).GetType("Result")!;
            Second = Load(_secondContext, image).GetType("Result")!;
        }

        private static Type DefineResultType(ModuleBuilder module, string name, Type dependency, string memberKind, JsonIgnoreCondition? ignoreCondition)
        {
            if (memberKind.StartsWith("Opaque", StringComparison.Ordinal))
            {
                return DefineOpaqueConverterType(module, name, dependency, memberKind);
            }

            if (memberKind.StartsWith("Shadow", StringComparison.Ordinal))
            {
                return DefineShadowedMemberType(module, name, dependency, memberKind, ignoreCondition);
            }

            if (memberKind is "IgnoredOverride" or "IncludedOverride" or "IgnoredOverrideWithBaseMember")
            {
                return DefineOverriddenPropertyType(module, name, dependency, memberKind);
            }

            if (memberKind is "Collection" or "Dictionary")
            {
                return DefineCollectionType(module, name, dependency, memberKind == "Dictionary");
            }

            var type = module.DefineType(name, TypeAttributes.Public, memberKind == "Base" ? dependency : typeof(object));
            CustomAttributeBuilder ConverterAttribute() => typeof(JsonConverterAttribute).IsAssignableFrom(dependency)
                ? new(dependency.GetConstructor(Type.EmptyTypes)!, [])
                : new(typeof(JsonConverterAttribute).GetConstructor([typeof(Type)])!, [dependency]);
            switch (memberKind)
            {
                case "PrivateGetter":
                case "SetterOnly":
                case "IncludedPrivateGetter":
                case "IncludedSetterOnly":
                    DefineSetterProperty(type, dependency, memberKind);
                    break;
                case "UnserializedField":
                    type.DefineField("Value", dependency, FieldAttributes.Public);
                    break;
                case "TypeConverter":
                    type.SetCustomAttribute(ConverterAttribute());
                    break;
                case "Property":
                case "PropertyConverter":
                    DefineResultProperty(type, dependency, memberKind == "PropertyConverter" ? ConverterAttribute() : null, ignoreCondition);
                    break;
                case "Field":
                case "FieldConverter":
                    DefineResultField(type, dependency, memberKind == "FieldConverter" ? ConverterAttribute() : null, ignoreCondition);
                    break;
            }

            return type.CreateType()!;
        }

        private static PropertyBuilder DefineResultProperty(TypeBuilder type, Type dependency, CustomAttributeBuilder? converterAttribute, JsonIgnoreCondition? ignoreCondition)
        {
            var propertyType = converterAttribute is null ? dependency : typeof(object);
            var property = type.DefineProperty("Value", PropertyAttributes.None, propertyType, null);
            if (ignoreCondition is { } condition)
            {
                property.SetCustomAttribute(IgnoreAttribute(condition));
            }

            if (converterAttribute is not null)
            {
                property.SetCustomAttribute(converterAttribute);
            }

            var getter = type.DefineMethod("get_Value",
                MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig, propertyType, Type.EmptyTypes);
            var il = getter.GetILGenerator();
            il.Emit(OpCodes.Ldnull);
            il.Emit(OpCodes.Ret);
            property.SetGetMethod(getter);
            return property;
        }

        private static Type DefineShadowedMemberType(ModuleBuilder module, string name, Type dependency, string memberKind, JsonIgnoreCondition? ignoreCondition)
        {
            var baseBuilder = module.DefineType($"{name}Base", TypeAttributes.Public);
            if (memberKind == "ShadowBaseField")
            {
                DefineResultField(baseBuilder, dependency, null, null);
            }
            else
            {
                DefineResultProperty(baseBuilder, dependency, null, null);
            }

            var derived = module.DefineType(name, TypeAttributes.Public, baseBuilder.CreateType()!);
            switch (memberKind)
            {
                case "ShadowSetterOnly":
                case "ShadowPrivateGetter":
                case "ShadowIncludedPrivateGetter":
                    DefineSetterProperty(derived, typeof(string), memberKind["Shadow".Length..]);
                    break;
                case "ShadowField":
                    DefineResultField(derived, typeof(string), null, ignoreCondition);
                    break;
                default:
                    var property = DefineResultProperty(derived, typeof(string), null, ignoreCondition);
                    if (memberKind == "ShadowRenamed")
                    {
                        property.SetCustomAttribute(new CustomAttributeBuilder(
                            typeof(JsonPropertyNameAttribute).GetConstructor([typeof(string)])!, ["Renamed"]));
                    }
                    break;
            }

            return derived.CreateType()!;
        }

        private static Type DefineOpaqueConverterType(ModuleBuilder module, string name, Type dependency, string memberKind)
        {
            if (memberKind == "OpaqueGenericProperty")
            {
                dependency = typeof(List<>).MakeGenericType(dependency);
            }
            else if (memberKind == "OpaqueInheritedProperty")
            {
                dependency = module.DefineType($"{name}ConvertedValue", TypeAttributes.Public, dependency).CreateType()!;
            }

            if (memberKind.StartsWith("OpaqueWrapped", StringComparison.Ordinal))
            {
                var wrapper = module.DefineType($"{name}ConvertedValue", TypeAttributes.Public);
                DefineResultProperty(wrapper, dependency, null, null);
                dependency = wrapper.CreateType()!;
            }

            var type = module.DefineType(name, TypeAttributes.Public);
            var converterType = memberKind.Contains("Forwarding", StringComparison.Ordinal) ? typeof(ForwardingObjectConverter) : typeof(ObjectConverter);
            var converter = new CustomAttributeBuilder(
                typeof(JsonConverterAttribute).GetConstructor([typeof(Type)])!, [converterType]);
            if (memberKind.EndsWith("Field", StringComparison.Ordinal))
            {
                var field = type.DefineField("Value", dependency, FieldAttributes.Public);
                field.SetCustomAttribute(new CustomAttributeBuilder(typeof(JsonIncludeAttribute).GetConstructor(Type.EmptyTypes)!, []));
                field.SetCustomAttribute(converter);
                var constructor = type.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, Type.EmptyTypes).GetILGenerator();
                constructor.Emit(OpCodes.Ldarg_0);
                constructor.Emit(OpCodes.Call, typeof(object).GetConstructor(Type.EmptyTypes)!);
                constructor.Emit(OpCodes.Ldarg_0);
                constructor.Emit(OpCodes.Newobj, dependency.GetConstructor(Type.EmptyTypes)!);
                constructor.Emit(OpCodes.Stfld, field);
                constructor.Emit(OpCodes.Ret);
            }
            else
            {
                var property = DefineInitializedProperty(type, "Value", dependency);
                if (memberKind == "OpaqueType")
                {
                    type.SetCustomAttribute(converter);
                }
                else
                {
                    property.SetCustomAttribute(converter);
                }
            }

            if (memberKind.EndsWith("WithVisibleMember", StringComparison.Ordinal))
            {
                DefineInitializedProperty(type, "Additional", dependency);
            }

            return type.CreateType()!;
        }

        private static PropertyBuilder DefineInitializedProperty(TypeBuilder type, string name, Type propertyType)
        {
            var property = type.DefineProperty(name, PropertyAttributes.None, propertyType, null);
            var getter = type.DefineMethod($"get_{name}", MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig,
                propertyType, Type.EmptyTypes);
            var il = getter.GetILGenerator();
            il.Emit(OpCodes.Newobj, propertyType.GetConstructor(Type.EmptyTypes)!);
            il.Emit(OpCodes.Ret);
            property.SetGetMethod(getter);
            return property;
        }

        private static void DefineResultField(TypeBuilder type, Type dependency, CustomAttributeBuilder? converterAttribute, JsonIgnoreCondition? ignoreCondition)
        {
            var fieldType = converterAttribute is null ? dependency : typeof(object);
            var field = type.DefineField("Value", fieldType, FieldAttributes.Public);
            if (ignoreCondition is { } condition)
            {
                field.SetCustomAttribute(IgnoreAttribute(condition));
            }

            field.SetCustomAttribute(new CustomAttributeBuilder(
                typeof(JsonIncludeAttribute).GetConstructor(Type.EmptyTypes)!, []));
            if (converterAttribute is not null)
            {
                field.SetCustomAttribute(converterAttribute);
            }
        }

        private static CustomAttributeBuilder IgnoreAttribute(JsonIgnoreCondition condition) => new(
            typeof(JsonIgnoreAttribute).GetConstructor(Type.EmptyTypes)!, [],
            [typeof(JsonIgnoreAttribute).GetProperty(nameof(JsonIgnoreAttribute.Condition))!], [condition]);

        private static Type DefineOverriddenPropertyType(ModuleBuilder module, string name, Type dependency, string memberKind)
        {
            static (MethodBuilder Getter, PropertyBuilder Property) DefineGetter(TypeBuilder type, string propertyName, Type propertyType)
            {
                var property = type.DefineProperty(propertyName, PropertyAttributes.None, propertyType, null);
                var getter = type.DefineMethod($"get_{propertyName}",
                    MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.SpecialName | MethodAttributes.HideBySig,
                    propertyType, Type.EmptyTypes);
                var il = getter.GetILGenerator();
                il.Emit(OpCodes.Ldnull);
                il.Emit(OpCodes.Ret);
                property.SetGetMethod(getter);
                return (getter, property);
            }

            var baseBuilder = module.DefineType($"{name}Base", TypeAttributes.Public);
            DefineGetter(baseBuilder, "Value", dependency);
            var baseType = baseBuilder.CreateType()!;
            var derived = module.DefineType(name, TypeAttributes.Public, baseType);
            var (overridden, property) = DefineGetter(derived, "Value", dependency);
            derived.DefineMethodOverride(overridden, baseType.GetMethod("get_Value")!);
            if (memberKind != "IncludedOverride")
            {
                property.SetCustomAttribute(new CustomAttributeBuilder(typeof(JsonIgnoreAttribute).GetConstructor(Type.EmptyTypes)!, []));
            }

            if (memberKind == "IgnoredOverrideWithBaseMember")
            {
                DefineGetter(derived, "BaseValue", baseType);
            }

            return derived.CreateType()!;
        }

        private static void DefineSetterProperty(TypeBuilder type, Type propertyType, string memberKind)
        {
            var property = type.DefineProperty("Value", PropertyAttributes.None, propertyType, null);
            if (memberKind.StartsWith("Included", StringComparison.Ordinal))
            {
                property.SetCustomAttribute(new CustomAttributeBuilder(
                    typeof(JsonIncludeAttribute).GetConstructor(Type.EmptyTypes)!, []));
            }

            var setter = type.DefineMethod("set_Value",
                MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig, typeof(void), [propertyType]);
            setter.GetILGenerator().Emit(OpCodes.Ret);
            property.SetSetMethod(setter);
            if (memberKind.EndsWith("PrivateGetter", StringComparison.Ordinal))
            {
                var getter = type.DefineMethod("get_Value",
                    MethodAttributes.Private | MethodAttributes.SpecialName | MethodAttributes.HideBySig, propertyType, Type.EmptyTypes);
                var il = getter.GetILGenerator();
                il.Emit(OpCodes.Ldnull);
                il.Emit(OpCodes.Ret);
                property.SetGetMethod(getter);
            }
        }

        private static Type DefineCollectionMemberType(ModuleBuilder module, Type dependency, string memberKind)
        {
            var parent = memberKind switch
            {
                var kind when kind.StartsWith("Dictionary", StringComparison.Ordinal) => typeof(Dictionary<string, int>),
                var kind when kind.StartsWith("ArrayList", StringComparison.Ordinal) => typeof(System.Collections.ArrayList),
                _ => typeof(List<int>),
            };
            var type = module.DefineType("CollectionBase", TypeAttributes.Public, parent);
            if (memberKind.Contains("Property", StringComparison.Ordinal))
            {
                DefineResultProperty(type, dependency, null, null);
            }
            else
            {
                DefineResultField(type, dependency, null, null);
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

        private static byte[] BuildConverterFactory(Type producedConverter, bool attribute)
        {
            var assembly = new PersistedAssemblyBuilder(new AssemblyName($"Factory_{Guid.NewGuid():N}"), typeof(object).Assembly);
            var baseType = attribute ? typeof(JsonConverterAttribute) : typeof(JsonConverterFactory);
            var type = assembly.DefineDynamicModule("Factory").DefineType("ConverterFactory", TypeAttributes.Public, baseType);
            type.DefineDefaultConstructor(MethodAttributes.Public);
            if (!attribute)
            {
                var canConvert = type.DefineMethod(nameof(JsonConverterFactory.CanConvert), MethodAttributes.Public | MethodAttributes.Virtual,
                    typeof(bool), [typeof(Type)]);
                var canConvertIl = canConvert.GetILGenerator();
                canConvertIl.Emit(OpCodes.Ldc_I4_1);
                canConvertIl.Emit(OpCodes.Ret);
                type.DefineMethodOverride(canConvert, baseType.GetMethod(nameof(JsonConverterFactory.CanConvert))!);
            }

            var create = type.DefineMethod(nameof(JsonConverterFactory.CreateConverter), MethodAttributes.Public | MethodAttributes.Virtual,
                typeof(JsonConverter), attribute ? [typeof(Type)] : [typeof(Type), typeof(JsonSerializerOptions)]);
            var il = create.GetILGenerator();
            il.Emit(OpCodes.Newobj, producedConverter.GetConstructor(Type.EmptyTypes)!);
            il.Emit(OpCodes.Ret);
            type.DefineMethodOverride(create, baseType.GetMethod(nameof(JsonConverterFactory.CreateConverter))!);
            type.CreateType();
            return Save(assembly);
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
