using System.Collections;
using System.Reflection;
using System.Text.Json;
using ModularPipelines.Distributed.Serialization;
using ModularPipelines.Distributed;
using ModularPipelines.Enums;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.Distributed.UnitTests.Serialization;

public class ModuleResultLoadContextTests
{
    public abstract class DefaultContextModule : Module<object>;

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Runtime_Plugin_Value_Resolves_Outside_Module_Context(bool collection)
    {
        using var builds = new ModuleLoadContextBuilds(typeof(Module<object>), identicalBuilds: false);
        var registry = new ModuleTypeRegistry();
        registry.Register(typeof(DefaultContextModule));
        var serializer = new ModuleResultSerializer(registry);
        foreach (var assembly in new[] { builds.First, builds.Second })
        {
            var payload = CreatePayload(serializer, assembly, collection, ModuleId.FromType(typeof(DefaultContextModule)).Value);
            await Assert.That(serializer.Deserialize(payload)!.ValueOrDefault!.GetType()).IsEqualTo(ValueType(assembly, collection));
        }
    }

    [Test]
    public async Task Runtime_Plugin_Value_With_Ambiguous_Load_Contexts_Is_Rejected()
    {
        using var builds = new ModuleLoadContextBuilds(typeof(Module<object>), identicalBuilds: true);
        var registry = new ModuleTypeRegistry();
        registry.Register(typeof(DefaultContextModule));
        var serializer = new ModuleResultSerializer(registry);
        var payload = CreatePayload(serializer, builds.First, false, ModuleId.FromType(typeof(DefaultContextModule)).Value);
        var exception = Assert.Throws<JsonException>(() => serializer.Deserialize(payload));
        await Assert.That(exception!.Message).Contains("ambiguous");
    }

    [Test]
    [Arguments(false, false)]
    [Arguments(false, true)]
    [Arguments(true, false)]
    [Arguments(true, true)]
    public async Task Runtime_Value_Uses_Registered_Module_Context(bool identicalBuilds, bool collection)
    {
        using var builds = new ModuleLoadContextBuilds(typeof(Module<object>), identicalBuilds);
        var first = builds.First;
        var second = builds.Second;
        var firstSerializer = CreateSerializer(first);
        var secondSerializer = CreateSerializer(second);
        var firstPayload = CreatePayload(firstSerializer, first, collection);
        var secondPayload = CreatePayload(secondSerializer, second, collection);

        await Assert.That(firstSerializer.Deserialize(firstPayload)!.ValueOrDefault!.GetType())
            .IsEqualTo(ValueType(first, collection));
        await Assert.That(secondSerializer.Deserialize(secondPayload)!.ValueOrDefault!.GetType())
            .IsEqualTo(ValueType(second, collection));
        if (identicalBuilds)
        {
            await Assert.That(firstSerializer.Deserialize(secondPayload)!.ValueOrDefault!.GetType())
                .IsEqualTo(ValueType(first, collection));
        }
        else
        {
            var exception = Assert.Throws<JsonException>(() => firstSerializer.Deserialize(secondPayload));
            await Assert.That(exception!.Message).Contains("build identity");
        }
    }

    private static ModuleResultSerializer CreateSerializer(Assembly assembly)
    {
        var registry = new ModuleTypeRegistry();
        registry.Register(assembly.GetType("ContextModule")!);
        return new ModuleResultSerializer(registry);
    }

    private static Type ValueType(Assembly assembly, bool collection) => collection
        ? typeof(List<>).MakeGenericType(assembly.GetType("RuntimeValue")!)
        : assembly.GetType("RuntimeValue")!;

    private static SerializedModuleResult CreatePayload(ModuleResultSerializer serializer, Assembly assembly, bool collection, string moduleId = "ContextModule")
    {
        var value = Activator.CreateInstance(ValueType(assembly, collection))!;
        if (collection)
        {
            ((IList) value).Add(Activator.CreateInstance(assembly.GetType("RuntimeValue")!));
        }

        var now = DateTimeOffset.UtcNow;
        return serializer.Serialize(new ModuleResult<object>.Success(value)
        {
            Name = "ContextModule",
            Duration = TimeSpan.Zero,
            StartTime = now,
            EndTime = now,
            Status = ModuleStatus.Succeeded,
        }, moduleId, 1);
    }
}
