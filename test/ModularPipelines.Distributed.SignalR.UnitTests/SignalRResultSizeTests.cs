using System.Text.Json;

namespace ModularPipelines.Distributed.SignalR.UnitTests;

public class SignalRResultSizeTests
{
    [Test]
    [Arguments(true)]
    [Arguments(false)]
    public async Task Oversized_Result_Completes_Lease_Without_Reconnecting(bool includeLease)
    {
        const long limit = 4096;
        await using var test = await SignalRTestMaster.StartAsync(options => options with
        {
            MaxMessageSizeBytes = limit,
            MaxReconnectAttempts = 2,
        });
        var worker = await test.ConnectWorkerAsync();
        var registration = SignalRTestMaster.Registration("worker-a");
        await worker.RegisterWorkerAsync(registration, CancellationToken.None);
        var reconnects = 0;
        worker.RegisterAgainAsync = _ =>
        {
            Interlocked.Increment(ref reconnects);
            return Task.CompletedTask;
        };
        var moduleId = new ModuleId("Signal.LargeResult");
        await test.Master.EnqueueModuleAsync(new ModuleAssignment
        {
            ModuleId = moduleId,
            RequiredCapabilities = CapabilityRequirement.None,
            PipelineSchemaVersion = "schema",
        }, CancellationToken.None);
        using var timeout = new CancellationTokenSource(TimeSpan.FromSeconds(10));
        var lease = await worker.DequeueModuleAsync(registration.WorkerId, new HashSet<Capability>(), timeout.Token);
        var result = new SerializedModuleResult
        {
            ModuleId = moduleId,
            WorkerId = registration.WorkerId,
            Payload = JsonSerializer.Serialize(string.Concat(Enumerable.Repeat("\"\\\n漢字🌍", 2048))),
            CompletedAt = DateTimeOffset.UtcNow,
        };

        await worker.PublishResultAsync(result, includeLease ? lease : null, timeout.Token);
        var received = await test.Master.WaitForResultAsync(moduleId, timeout.Token);

        using var payload = JsonDocument.Parse(received.Payload);
        await Assert.That(payload.RootElement.GetProperty("$type").GetString()).IsEqualTo("Failure");
        await Assert.That(received.Payload).Contains(nameof(SignalRDistributedOptions.MaxMessageSizeBytes));
        await Assert.That(await test.Master.GetActiveLeasesAsync(timeout.Token)).IsEmpty();
        await Assert.That(await test.Master.RequeueExpiredLeasesAsync(timeout.Token)).IsEmpty();
        await Assert.That(reconnects).IsEqualTo(0);
        await Assert.That(await worker.GetMasterStateAsync(timeout.Token)).IsEqualTo(DistributedMasterState.Running);

        // A subsequent ordinary publication proves the same connection remains usable.
        var next = result with { ModuleId = new ModuleId("Signal.Next"), Payload = "small" };
        await worker.PublishResultAsync(next, null, timeout.Token);
        await Assert.That((await test.Master.WaitForResultAsync(next.ModuleId, timeout.Token)).Payload).IsEqualTo("small");
    }
}
