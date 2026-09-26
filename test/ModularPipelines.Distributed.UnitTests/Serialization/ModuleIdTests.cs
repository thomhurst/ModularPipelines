using System.Text.Json;
using ModularPipelines.Attributes;
using ModularPipelines.Distributed.Serialization;
using ModularPipelines.Modules;

namespace ModularPipelines.Distributed.UnitTests.Serialization;

public class ModuleIdTests
{
    [ModuleId("build")]
    private sealed class OriginalModule : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) =>
            Task.FromResult("output");
    }

    [ModuleId("build")]
    private sealed class RenamedModule : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) =>
            Task.FromResult("output");
    }

    private sealed class GenericModule<T> : Module<T>
    {
        protected internal override Task<T> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) =>
            throw new NotSupportedException();
    }

    [Test]
    public async Task Generic_Module_Identity_Omits_Assembly_Versions()
    {
        var id = ModuleId.FromType(typeof(GenericModule<List<string>>));
        await Assert.That(id.Value).DoesNotContain("Version=");
        await Assert.That(id.Value).DoesNotContain("PublicKeyToken=");
        await Assert.That(id).IsNotEqualTo(ModuleId.FromType(typeof(GenericModule<List<int>>)));
    }

    [Test]
    public async Task Identifiers_RoundTrip_As_Values_And_Dictionary_Keys()
    {
        var id = new ModuleId("build:linux");
        var json = JsonSerializer.Serialize(new Dictionary<ModuleId, ModuleId> { [id] = id });
        var result = JsonSerializer.Deserialize<Dictionary<ModuleId, ModuleId>>(json);
        await Assert.That(json).IsEqualTo("{\"build:linux\":\"build:linux\"}");
        await Assert.That(result![id]).IsEqualTo(id);
    }

    [Test]
    [Arguments("\"\"")]
    [Arguments("\" \"")]
    [Arguments("null")]
    [Arguments("1")]
    public async Task Invalid_Json_Identity_Is_Rejected(string json)
    {
        await Assert.That(() => JsonSerializer.Deserialize<ModuleId>(json)).Throws<JsonException>();
    }

    [Test]
    public async Task Default_Identity_Cannot_Be_Written()
    {
        await Assert.That(() => JsonSerializer.Serialize(default(ModuleId))).Throws<JsonException>();
        await Assert.That(() => JsonSerializer.Serialize(new Dictionary<ModuleId, int> { [default] = 1 }))
            .Throws<JsonException>();
    }

    [Test]
    public async Task Renamed_Module_Resolves_Through_Stable_Identity()
    {
        var producer = new ModuleTypeRegistry();
        producer.Register(typeof(OriginalModule));
        var consumer = new ModuleTypeRegistry();
        consumer.Register(typeof(RenamedModule));
        var now = DateTimeOffset.UtcNow;
        var result = new ModuleResult<string>.Success("output")
        {
            Name = "original",
            StartTime = now,
            EndTime = now,
            Duration = TimeSpan.Zero,
            Status = ModuleStatus.Succeeded,
        };
        var serialized = new ModuleResultSerializer(producer).Serialize(result,
            ModuleId.FromType(typeof(OriginalModule)), 1);
        var received = new ModuleResultSerializer(consumer).Deserialize(serialized);

        await Assert.That(received!.ValueOrDefault).IsEqualTo("output");
        await Assert.That(((ModuleResult) received).ModuleType).IsEqualTo(typeof(RenamedModule));
        await Assert.That(serialized.Payload).DoesNotContain("$valueType");
        await Assert.That(producer.GetPipelineSchemaVersion()).IsEqualTo(consumer.GetPipelineSchemaVersion());
    }

    [Test]
    public async Task Schema_Failure_Can_Be_Published_Without_Remote_Module_Type()
    {
        var consumer = new ModuleTypeRegistry();
        consumer.Register(typeof(RenamedModule));
        var failure = new ModuleResultSerializer(new ModuleTypeRegistry()).SerializeFailure("build",
            new InvalidOperationException("Pipeline schema mismatch"), 2);
        var received = new ModuleResultSerializer(consumer).Deserialize(failure);
        await Assert.That(received!.Status).IsEqualTo(ModuleStatus.Failed);
        await Assert.That(received.ExceptionOrDefault!.Message).Contains("schema mismatch");
    }
}
