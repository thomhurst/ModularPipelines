using ModularPipelines.Distributed;
using TUnit.Assertions;
using TUnit.Assertions.Extensions;

namespace ModularPipelines.TestHelpers;

public static class LeaseReconciliationChecks
{
    public static async Task ReconnectBeforeExpiryAsync(
        IDistributedMasterCoordinator master, IDistributedWorkerCoordinator worker, string? runId = null)
    {
        var registration = Registration("original", runId);
        await worker.RegisterWorkerAsync(registration, CancellationToken.None);
        var lease = await ClaimAsync(master, worker, registration.WorkerId);
        await worker.RegisterWorkerAsync(registration, CancellationToken.None);
        var lost = await worker.ReconcileHeartbeatAsync(Status(registration, lease), [lease], CancellationToken.None);
        await Assert.That(lost).IsEmpty();
        await worker.PublishResultAsync(Result(lease, "original"), lease, CancellationToken.None);
        await Assert.That((await master.WaitForResultAsync(lease.Assignment.ModuleId, CancellationToken.None)).Payload)
            .IsEqualTo("original");
    }

    public static async Task ReconnectAfterReassignmentAsync(
        IDistributedMasterCoordinator master, Func<Task> expire,
        bool sameWorker = false, IDistributedWorkerCoordinator? worker = null,
        IDistributedWorkerCoordinator? replacement = null, string? runId = null)
    {
        worker ??= master;
        replacement ??= master;
        var registration = Registration("original", runId);
        await worker.RegisterWorkerAsync(registration, CancellationToken.None);
        var old = await ClaimAsync(master, worker, registration.WorkerId);
        await expire();
        await worker.RegisterWorkerAsync(registration, CancellationToken.None);
        await Assert.That(await worker.ReconcileHeartbeatAsync(Status(registration, old), [old], CancellationToken.None))
            .IsEquivalentTo([old.LeaseId]);
        var pending = master.WaitForResultAsync(old.Assignment.ModuleId, CancellationToken.None);
        await worker.PublishResultAsync(Result(old, "expired"), old, CancellationToken.None);
        await Assert.That(pending.IsCompleted).IsFalse();
        await Assert.That(await master.RequeueExpiredLeasesAsync(CancellationToken.None)).Contains(old.Assignment.ModuleId);
        var replacementRegistration = sameWorker ? registration : Registration("replacement", runId);
        await replacement.RegisterWorkerAsync(replacementRegistration, CancellationToken.None);
        var current = (await replacement.DequeueModuleAsync(
            replacementRegistration.WorkerId, new HashSet<Capability>(), CancellationToken.None))!;
        await Assert.That(current.LeaseId).IsNotEqualTo(old.LeaseId);
        await worker.RegisterWorkerAsync(registration, CancellationToken.None);
        var lost = await worker.ReconcileHeartbeatAsync(Status(registration, old), [old], CancellationToken.None);
        await Assert.That(lost).IsEquivalentTo([old.LeaseId]);
        await worker.PublishResultAsync(Result(old, "stale"), old, CancellationToken.None);
        await worker.PublishResultAsync(Result(old, "invalid"), old with { LeaseId = string.Empty }, CancellationToken.None);
        await Assert.That(pending.IsCompleted).IsFalse();
        await Assert.That((await master.GetActiveLeasesAsync(CancellationToken.None)).Select(lease => lease.LeaseId))
            .IsEquivalentTo([current.LeaseId]);
        await replacement.PublishResultAsync(Result(current, "replacement"), current, CancellationToken.None);
        await Assert.That((await pending.WaitAsync(TimeSpan.FromSeconds(10))).Payload).IsEqualTo("replacement");
    }

    private static WorkerRegistration Registration(string worker, string? runId) => new()
    {
        WorkerId = new WorkerId(worker),
        RegisteredAt = DateTimeOffset.UtcNow,
        Capabilities = [],
        RunId = runId,
    };

    private static WorkerStatus Status(WorkerRegistration registration, ModuleLease lease) => new()
    {
        WorkerId = registration.WorkerId,
        RunId = registration.RunId,
        InFlightModules = [lease.Assignment.ModuleId],
    };

    private static async Task<ModuleLease> ClaimAsync(
        IDistributedMasterCoordinator master, IDistributedWorkerCoordinator worker, WorkerId workerId)
    {
        await master.EnqueueModuleAsync(new ModuleAssignment
        {
            ModuleId = new ModuleId("Lease.Reconciliation"),
            RequiredCapabilities = CapabilityRequirement.None,
            PipelineSchemaVersion = "test",
        }, CancellationToken.None);
        return (await worker.DequeueModuleAsync(workerId, new HashSet<Capability>(), CancellationToken.None))!;
    }

    private static SerializedModuleResult Result(ModuleLease lease, string payload) => new()
    {
        ModuleId = lease.Assignment.ModuleId,
        WorkerId = lease.WorkerId,
        Payload = payload,
        CompletedAt = DateTimeOffset.UtcNow,
    };
}
