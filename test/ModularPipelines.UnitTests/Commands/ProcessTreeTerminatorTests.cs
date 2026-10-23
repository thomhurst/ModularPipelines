using ModularPipelines.Context;
using ModularPipelines.Helpers.Internal;
using ModularPipelines.Logging;
using ModularPipelines.TestHelpers;
using ModularPipelines.UnitTests.Attributes;
using ModularPipelines.UnitTests.Helpers;
using Moq;

namespace ModularPipelines.UnitTests.Commands;

[TUnit.Core.ParallelLimiter<ProcessFixtureParallelLimit>]
public class ProcessTreeTerminatorTests : TestBase
{
    [Test]
    [RequiresTool("dotnet")]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Mismatched_Start_Time_Does_Not_Interrupt_Or_Kill(bool captureAfterCancellation)
    {
        await using var fixture = new ProcessTreeFixture();
        fixture.Start(await GetService<ICommandContext>(), "interruptible-parent", TimeSpan.FromMilliseconds(50));
        var grandchild = await fixture.WaitForReadyProcessAsync("interruptible-grandchild", TimeSpan.FromSeconds(10));
        using var jobs = new WindowsJobLifetime();
        var logger = new Mock<ICommandLogger>();
        using var terminator = new Command.ProcessTreeTerminator(logger.Object, jobs);
        if (captureAfterCancellation)
        {
            terminator.Kill();
        }

        // Model PID reuse without forcing the OS to recycle a PID: the live process has
        // the captured PID, but it started at a different time from the original descendant.
        terminator.CaptureDescendant(new(grandchild.Id, grandchild.StartTime.AddSeconds(-1)));
        await Assert.That(terminator.HasRunningProcesses()).IsFalse();
        terminator.Kill();

        await Assert.ThrowsAsync<TimeoutException>(() => grandchild.WaitForExitAsync().WaitAsync(TimeSpan.FromMilliseconds(250)));
        await Assert.That(File.Exists(Path.Combine(fixture.DirectoryPath, "interruptible-grandchild-interrupted.ready"))).IsFalse();
        logger.Verify(logger => logger.LogProcessTermination(
            It.IsAny<ProcessTerminationTarget>(),
            It.IsAny<ProcessTerminationReason>(),
            It.IsAny<ProcessTerminationMethod>()), Times.Never());
    }

    [Test]
    [UnixOnlyTest]
    [RequiresTool("dotnet")]
    public async Task Mismatched_Start_Time_Does_Not_Discover_Unrelated_Children()
    {
        await using var fixture = new ProcessTreeFixture();
        fixture.Start(await GetService<ICommandContext>(), "parent", TimeSpan.FromMilliseconds(50));
        var parent = await fixture.WaitForReadyProcessAsync("parent", TimeSpan.FromSeconds(10));
        var child = await fixture.WaitForReadyProcessAsync("child", TimeSpan.FromSeconds(10));
        using var jobs = new WindowsJobLifetime();
        using var terminator = new Command.ProcessTreeTerminator(Mock.Of<ICommandLogger>(), jobs);
        terminator.CaptureDescendant(new(parent.Id, parent.StartTime.AddSeconds(-1)));

        terminator.BeginGracefulShutdown();
        terminator.Kill();

        await Assert.ThrowsAsync<TimeoutException>(() => parent.WaitForExitAsync().WaitAsync(TimeSpan.FromMilliseconds(250)));
        await Assert.ThrowsAsync<TimeoutException>(() => child.WaitForExitAsync().WaitAsync(TimeSpan.FromMilliseconds(250)));
        await Assert.That(terminator.HasRunningProcesses()).IsFalse();
    }

    [Test]
    [RequiresTool("dotnet")]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Matching_Start_Time_Kills_Descendant(bool captureAfterCancellation)
    {
        await using var fixture = new ProcessTreeFixture();
        fixture.Start(await GetService<ICommandContext>(), "gated-parent", TimeSpan.FromMilliseconds(50));
        await fixture.TriggerAsync("start-child");
        var child = await fixture.WaitForReadyProcessAsync("child", TimeSpan.FromSeconds(10));
        using var jobs = new WindowsJobLifetime();
        var logger = new Mock<ICommandLogger>();
        using var terminator = new Command.ProcessTreeTerminator(logger.Object, jobs);
        if (captureAfterCancellation)
        {
            terminator.Kill();
        }

        terminator.CaptureDescendant(new(child.Id, child.StartTime));
        if (!captureAfterCancellation)
        {
            await Assert.That(terminator.HasRunningProcesses()).IsTrue();
            terminator.Kill();
        }

        await child.WaitForExitAsync().WaitAsync(TimeSpan.FromSeconds(5));
        await Assert.That(terminator.HasRunningProcesses()).IsFalse();
        var expectedReason = captureAfterCancellation
            ? ProcessTerminationReason.DescendantCapturedAfterCancellation
            : ProcessTerminationReason.ForcefulCancellation;
        logger.Verify(logger => logger.LogProcessTermination(
            It.Is<ProcessTerminationTarget>(target => target.ProcessId == child.Id),
            expectedReason,
            ProcessTerminationMethod.ProcessTree), Times.Once());
    }

    [Test]
    [RequiresTool("dotnet")]
    public async Task Exited_Descendant_Is_Ignored()
    {
        await using var fixture = new ProcessTreeFixture();
        fixture.Start(await GetService<ICommandContext>(), "gated-parent", TimeSpan.FromMilliseconds(50));
        await fixture.TriggerAsync("start-child");
        var child = await fixture.WaitForReadyProcessAsync("child", TimeSpan.FromSeconds(10));
        var identity = new Command.ProcessTreeTerminator.CapturedDescendant(child.Id, child.StartTime);
        child.Kill();
        await child.WaitForExitAsync().WaitAsync(TimeSpan.FromSeconds(5));
        using var jobs = new WindowsJobLifetime();
        using var terminator = new Command.ProcessTreeTerminator(Mock.Of<ICommandLogger>(), jobs);

        terminator.CaptureDescendant(identity);
        terminator.Kill();

        await Assert.That(terminator.HasRunningProcesses()).IsFalse();
    }
}
