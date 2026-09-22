using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.Loader;
using System.Text.Json;
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
    public async Task Recursive_Generic_Base_RoundTrips_Without_Fingerprint_Recursion(Type type)
    {
        var json = SerializeValue(type);
        var result = JsonSerializer.Deserialize<ModuleResult<object>>(json);
        await Assert.That(result!.Value.GetType()).IsEqualTo(type);
    }

    private static string SerializeValue(Type type) => JsonSerializer.Serialize(
        new ModuleResult<object>.Success(Activator.CreateInstance(type)!)
        {
            Name = "InheritedResult",
            Duration = TimeSpan.Zero,
            StartTime = DateTimeOffset.UtcNow,
            EndTime = DateTimeOffset.UtcNow,
            Status = ModuleStatus.Succeeded,
        });

    private sealed class ResultBuilds : IDisposable
    {
        private readonly AssemblyLoadContext _firstContext = new(null, isCollectible: true);
        private readonly AssemblyLoadContext _secondContext = new(null, isCollectible: true);

        public Type First { get; }

        public Type Second { get; }

        public ResultBuilds(int intermediateLevels)
        {
            var baseName = $"ResultBase_{Guid.NewGuid():N}";
            var firstBase = Load(_firstContext, BuildBase(baseName, "Original")).GetType("ResultBase")!;
            Load(_secondContext, BuildBase(baseName, "Changed"));

            var derived = new PersistedAssemblyBuilder(new AssemblyName($"Derived_{Guid.NewGuid():N}"), typeof(object).Assembly);
            var module = derived.DefineDynamicModule("Derived");
            var parent = firstBase;
            for (var level = 0; level < intermediateLevels; level++)
            {
                parent = module.DefineType($"Intermediate{level}", TypeAttributes.Public, parent).CreateType()!;
            }

            module.DefineType("Result", TypeAttributes.Public, parent).CreateType();
            var image = Save(derived);
            First = Load(_firstContext, image).GetType("Result")!;
            Second = Load(_secondContext, image).GetType("Result")!;
        }

        private static byte[] BuildBase(string name, string propertyName)
        {
            var assembly = new PersistedAssemblyBuilder(new AssemblyName(name), typeof(object).Assembly);
            var type = assembly.DefineDynamicModule(name).DefineType("ResultBase", TypeAttributes.Public);
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
