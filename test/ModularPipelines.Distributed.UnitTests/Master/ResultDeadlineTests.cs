using Microsoft.Extensions.Time.Testing;
using ModularPipelines.Distributed;
using ModularPipelines.Distributed.Master;

namespace ModularPipelines.Distributed.UnitTests.Master;

public class ResultDeadlineTests
{
    private static readonly TimeSpan Backstop = TimeSpan.FromMinutes(45);

    [Test]
    public async Task Unclaimed_Assignment_Has_No_Result_Deadline()
    {
        var clock = new FakeTimeProvider();
        using var deadline = new DistributedModuleExecutor.ResultDeadline(Backstop, null, clock);

        clock.Advance(Backstop * 2);

        await Assert.That(deadline.Token.IsCancellationRequested).IsFalse();
    }

    [Test]
    public async Task Repeated_Observation_Of_Same_Lease_Does_Not_Extend_Deadline()
    {
        var clock = new FakeTimeProvider();
        using var deadline = new DistributedModuleExecutor.ResultDeadline(Backstop, null, clock);
        deadline.Arm("lease-a");
        clock.Advance(Backstop / 2);
        deadline.Arm("lease-a");
        clock.Advance(Backstop / 2);

        await Assert.That(deadline.Token.IsCancellationRequested).IsTrue();
    }

    [Test]
    public async Task Replacement_Lease_Gets_Full_Deadline_Even_Without_Observed_Requeue()
    {
        var clock = new FakeTimeProvider();
        using var deadline = new DistributedModuleExecutor.ResultDeadline(Backstop, null, clock);
        deadline.Arm("lease-a");
        clock.Advance(Backstop - TimeSpan.FromMinutes(1));
        deadline.Arm("lease-b");
        clock.Advance(Backstop - TimeSpan.FromMinutes(1));

        await Assert.That(deadline.Token.IsCancellationRequested).IsFalse();
        clock.Advance(TimeSpan.FromMinutes(1));
        await Assert.That(deadline.Token.IsCancellationRequested).IsTrue();
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Requeued_Assignment_Waits_For_Claim_Then_Gets_Full_Deadline(bool workerOnly)
    {
        var clock = new FakeTimeProvider();
        using var deadline = new DistributedModuleExecutor.ResultDeadline(
            Backstop,
            workerOnly ? CapabilityRequirement.AllOf(Capability.Gpu) : null,
            clock);

        for (var claim = 0; claim < 3; claim++)
        {
            deadline.Arm($"lease-{claim}");
            await Assert.That(deadline.RequeuedAt).IsNull();
            clock.Advance(Backstop - TimeSpan.FromMinutes(1));
            await Assert.That(deadline.Token.IsCancellationRequested).IsFalse();
            deadline.MarkRequeued(clock.GetUtcNow());
            clock.Advance(Backstop * 2);
            await Assert.That(deadline.Token.IsCancellationRequested).IsFalse();
        }

        deadline.Arm("final-lease");
        clock.Advance(Backstop);
        await Assert.That(deadline.Token.IsCancellationRequested).IsTrue();
    }

    [Test]
    public async Task Claim_Observed_Before_Requeue_Notification_Resumes_On_Next_Observation()
    {
        var clock = new FakeTimeProvider();
        using var deadline = new DistributedModuleExecutor.ResultDeadline(Backstop, null, clock);
        deadline.Arm("lease-a");
        clock.Advance(Backstop / 2);

        // The master can claim the replacement before maintenance reports the expired lease.
        deadline.Arm("lease-b");
        deadline.MarkRequeued(clock.GetUtcNow());
        clock.Advance(Backstop * 2);
        await Assert.That(deadline.Token.IsCancellationRequested).IsFalse();

        deadline.Arm("lease-b");
        clock.Advance(Backstop - TimeSpan.FromMinutes(1));
        await Assert.That(deadline.Token.IsCancellationRequested).IsFalse();
        clock.Advance(TimeSpan.FromMinutes(1));
        await Assert.That(deadline.Token.IsCancellationRequested).IsTrue();
    }

    [Test]
    public async Task Infinite_Deadline_Still_Tracks_Stranded_Work()
    {
        var clock = new FakeTimeProvider();
        using var deadline = new DistributedModuleExecutor.ResultDeadline(
            null, CapabilityRequirement.AllOf(Capability.Gpu), clock);
        deadline.Arm("lease-a");
        deadline.MarkRequeued(clock.GetUtcNow());
        clock.Advance(Backstop * 2);
        await Assert.That(deadline.Token.IsCancellationRequested).IsFalse();
        await Assert.That(deadline.RequeuedAt).IsNotNull();

        deadline.Arm("lease-b");
        await Assert.That(deadline.RequeuedAt).IsNull();
        var reason = new InvalidOperationException("No capable workers remain.");
        deadline.Strand(reason);
        await Assert.That(deadline.Token.IsCancellationRequested).IsTrue();
        await Assert.That(deadline.StrandReason).IsSameReferenceAs(reason);
    }
}
