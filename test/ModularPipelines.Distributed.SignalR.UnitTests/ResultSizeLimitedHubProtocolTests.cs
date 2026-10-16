using System.Buffers;
using System.Text.Encodings.Web;
using System.Text.Json;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Protocol;
using ModularPipelines.Distributed.Serialization;
using ModularPipelines.Distributed.SignalR.Coordination;
using ModularPipelines.Distributed.SignalR.Hub;
using ModularPipelines.Enums;
using MsOptions = Microsoft.Extensions.Options.Options;

namespace ModularPipelines.Distributed.SignalR.UnitTests;

public class ResultSizeLimitedHubProtocolTests
{
    private static readonly JsonSerializerOptions PayloadOptions = new()
    {
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
    };

    [Test]
    [Arguments("plain", -1)]
    [Arguments("plain", 0)]
    [Arguments("plain", 1)]
    [Arguments("\"\\\n\t", -1)]
    [Arguments("\"\\\n\t", 0)]
    [Arguments("\"\\\n\t", 1)]
    [Arguments("漢字é🌍", -1)]
    [Arguments("漢字é🌍", 0)]
    [Arguments("漢字é🌍", 1)]
    public async Task Compares_Complete_Utf8_Invocation_Against_Limit(string text, int headroom)
    {
        var inner = CreateJsonProtocol();
        var invocation = Invocation(text);
        var original = inner.GetMessageBytes(invocation);
        var limit = original.Length + headroom;
        var protocol = new ResultSizeLimitedHubProtocol(inner, limit);

        var bytes = protocol.GetMessageBytes(invocation);

        await Assert.That(bytes.Length).IsLessThanOrEqualTo(limit);
        await Assert.That(bytes.Span[^1]).IsEqualTo((byte) 0x1e);
        if (headroom >= 0)
        {
            await Assert.That(bytes.Span.SequenceEqual(original.Span)).IsTrue();
            var output = new ArrayBufferWriter<byte>();
            protocol.WriteMessage(invocation, output);
            await Assert.That(output.WrittenSpan.SequenceEqual(original.Span)).IsTrue();
            return;
        }

        using var json = JsonDocument.Parse(bytes[..^1]);
        await Assert.That(json.RootElement.GetProperty("invocationId").GetString()).IsEqualTo(invocation.InvocationId);
        await Assert.That(json.RootElement.GetProperty("headers").GetProperty("trace").GetString()).IsEqualTo("trace-value");
        var received = json.RootElement.GetProperty("arguments")[0].Deserialize<SerializedModuleResult>()!;
        var failure = JsonSerializer.Deserialize<ModuleResult<string>>(received.Payload, ModuleResultSerializer.CreateOptions())!;
        await Assert.That(failure.Status).IsEqualTo(ModuleStatus.Failed);
        await Assert.That(failure.ExceptionOrDefault!.Message).Contains($"{original.Length} bytes");
        await Assert.That(failure.ExceptionOrDefault!.Message).Contains(nameof(SignalRDistributedOptions.MaxMessageSizeBytes));
        await Assert.That(received.ModuleId).IsEqualTo(((SerializedModuleResult) invocation.Arguments[0]!).ModuleId);
        await Assert.That(received.WorkerId).IsEqualTo(((SerializedModuleResult) invocation.Arguments[0]!).WorkerId);
        await Assert.That(received.CommandCount).IsEqualTo(7);
    }

    [Test]
    public async Task Failure_Drops_Large_Lease_Metadata_And_Fits_Before_Write()
    {
        var invocation = Invocation("large");
        var lease = (ModuleLease) invocation.Arguments[1]!;
        invocation.Arguments[1] = lease with
        {
            Assignment = lease.Assignment with { SatisfiedConditionGroups = [new string('x', 20_000)] },
        };
        const int limit = 4096;
        var output = new ArrayBufferWriter<byte>();
        new ResultSizeLimitedHubProtocol(CreateJsonProtocol(), limit).WriteMessage(invocation, output);

        await Assert.That(output.WrittenCount).IsLessThanOrEqualTo(limit);
        using var json = JsonDocument.Parse(output.WrittenMemory[..^1]);
        var receivedLease = json.RootElement.GetProperty("arguments")[1].Deserialize<ModuleLease>()!;
        await Assert.That(receivedLease.LeaseId).IsEqualTo(lease.LeaseId);
        await Assert.That(receivedLease.WorkerId).IsEqualTo(lease.WorkerId);
        await Assert.That(receivedLease.Assignment.ModuleId).IsEqualTo(lease.Assignment.ModuleId);
        await Assert.That(receivedLease.Assignment.SatisfiedConditionGroups).IsEmpty();
    }

    [Test]
    public async Task Impossible_Limit_Throws_Without_Writing_Any_Bytes()
    {
        var output = new ArrayBufferWriter<byte>();
        var protocol = new ResultSizeLimitedHubProtocol(CreateJsonProtocol(), 1);

        await Assert.That(() => protocol.WriteMessage(Invocation("large"), output))
            .Throws<HubException>()
            .WithMessageContaining(nameof(SignalRDistributedOptions.MaxMessageSizeBytes));
        await Assert.That(output.WrittenCount).IsEqualTo(0);
    }

    [Test]
    public async Task Other_Messages_Keep_Their_Protocol_Encoding()
    {
        var inner = CreateJsonProtocol();
        var invocation = new InvocationMessage("12345", HubMethodNames.Heartbeat, [new string('x', 1024)]);
        var output = new ArrayBufferWriter<byte>();
        var protocol = new ResultSizeLimitedHubProtocol(inner, 1);

        protocol.WriteMessage(invocation, output);

        await Assert.That(output.WrittenSpan.SequenceEqual(inner.GetMessageBytes(invocation).Span)).IsTrue();
    }

    private static JsonHubProtocol CreateJsonProtocol()
    {
        var options = new JsonHubProtocolOptions();
        options.PayloadSerializerOptions.PropertyNamingPolicy = null;
        options.PayloadSerializerOptions.PropertyNameCaseInsensitive = true;
        return new JsonHubProtocol(MsOptions.Create(options));
    }

    private static InvocationMessage Invocation(string text)
    {
        var result = new SerializedModuleResult
        {
            ModuleId = new ModuleId("Signal.Module"),
            WorkerId = new WorkerId("worker-a"),
            Payload = JsonSerializer.Serialize(string.Concat(Enumerable.Repeat(text, 2048)), PayloadOptions),
            CompletedAt = DateTimeOffset.UnixEpoch,
            CommandCount = 7,
        };
        var lease = new ModuleLease
        {
            LeaseId = "lease-id",
            WorkerId = result.WorkerId,
            Assignment = new ModuleAssignment
            {
                ModuleId = result.ModuleId,
                RequiredCapabilities = CapabilityRequirement.None,
                PipelineSchemaVersion = "schema",
            },
        };
        return new InvocationMessage("1234567890123456789", HubMethodNames.PublishResult, [result, lease])
        {
            Headers = new Dictionary<string, string> { ["trace"] = "trace-value" },
        };
    }
}
