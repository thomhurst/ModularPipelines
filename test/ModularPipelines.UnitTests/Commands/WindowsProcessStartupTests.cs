using System.Diagnostics;
using System.Text;
using ModularPipelines.Context;
using ModularPipelines.Helpers.Internal;
using ModularPipelines.Logging;
using ModularPipelines.ProcessTestHost;
using ModularPipelines.TestHelpers;
using ModularPipelines.UnitTests.Attributes;
using ModularPipelines.UnitTests.Helpers;
using Moq;

namespace ModularPipelines.UnitTests.Commands;

[WindowsOnlyTest]
[TUnit.Core.ParallelGroup(nameof(ProcessFixtureParallelLimit))]
[TUnit.Core.ParallelLimiter<ProcessFixtureParallelLimit>]
public class WindowsProcessStartupTests : TestBase
{
    [Test]
    [RequiresTool("dotnet")]
    public async Task Command_Code_Runs_Only_After_Startup_Callback()
    {
        await using var fixture = new ProcessTreeFixture();
        var marker = Path.Combine(fixture.DirectoryPath, "started");
        var runtimeConfiguration = Path.ChangeExtension(typeof(CommandTests).Assembly.Location, ".runtimeconfig.json");
        var command = new ProcessCommand("dotnet",
            ["exec", "--runtimeconfig", runtimeConfiguration, typeof(Host).Assembly.Location, "startup-marker", marker], string.Empty,
            Environment.CurrentDirectory, new Dictionary<string, string?>(), null);
        var output = new BoundedCommandOutputBuffer(1024);
        var error = new BoundedCommandOutputBuffer(1024);
        var attached = false;

        var result = await ProcessRunner.RunAsync(command,
            new CommandOutputTarget(output, null, null), new CommandOutputTarget(error, null, null),
            Encoding.UTF8, TimeSpan.FromSeconds(5), process =>
            {
                // Without suspension a fast child can spawn descendants before its
                // per-command job is attached, even with KillOnParentExit enabled.
                if (OperatingSystem.IsWindows() && !process.StartInfo.StartSuspended)
                {
                    throw new InvalidOperationException("Windows startup must install command containment before child code runs.");
                }

                // Keep the startup callback open while a normally scheduled child could run.
                Thread.Sleep(100);
                if (File.Exists(marker))
                {
                    throw new InvalidOperationException("Child code ran before command containment was attached.");
                }

                attached = true;
            }, CancellationToken.None);

        await Assert.That(attached).IsTrue();
        await Assert.That(File.Exists(marker)).IsTrue();
        await Assert.That(result.ExitCode).IsEqualTo(0);
        await Assert.That(output.ToString()).Contains("startup-output");
        await Assert.That(error.ToString()).Contains("startup-error");
    }

    [Test]
    public async Task Startup_Callback_Failure_Terminates_Suspended_Child()
    {
        var command = new ProcessCommand(Path.Combine(Environment.SystemDirectory, "cmd.exe"),
            ["/d", "/c", "exit 0"], string.Empty,
            Environment.CurrentDirectory, new Dictionary<string, string?>(), null);
        var output = new CommandOutputTarget(new BoundedCommandOutputBuffer(1024), null, null);
        Process? observer = null;
        try
        {
            await Assert.ThrowsAsync<InvalidOperationException>(() => ProcessRunner.RunAsync(command,
                output, output, Encoding.UTF8, TimeSpan.FromSeconds(5), process =>
                {
                    observer = Process.GetProcessById(process.Id);
                    _ = observer.SafeHandle;
                    throw new InvalidOperationException("startup callback failed");
                }, CancellationToken.None));

            await observer!.WaitForExitAsync().WaitAsync(TimeSpan.FromSeconds(5));
        }
        finally
        {
            using (observer)
            {
                if (observer is { HasExited: false })
                {
                    observer.Kill();
                    await observer.WaitForExitAsync().WaitAsync(TimeSpan.FromSeconds(5));
                }
            }
        }
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Job_Attachment_Failure_Terminates_Child_Before_It_Runs(bool cancelBeforeAttachment)
    {
        await using var fixture = new ProcessTreeFixture();
        var marker = Path.Combine(fixture.DirectoryPath, "uncontained-startup");
        var command = new ProcessCommand(Path.Combine(Environment.SystemDirectory, "cmd.exe"),
            ["/d", "/c", $"echo started>\"{marker}\""], string.Empty,
            Environment.CurrentDirectory, new Dictionary<string, string?>(), null);
        var output = new CommandOutputTarget(new BoundedCommandOutputBuffer(1024), null, null);
        using var jobs = new WindowsJobLifetime();
        using var terminator = new Command.ProcessTreeTerminator(Mock.Of<ICommandLogger>(), jobs, _ => null);
        if (cancelBeforeAttachment)
        {
            terminator.Kill();
        }

        Process? observer = null;
        try
        {
            await Assert.ThrowsAsync<InvalidOperationException>(() => ProcessRunner.RunAsync(command,
                output, output, Encoding.UTF8, TimeSpan.FromSeconds(5), process =>
                {
                    observer = Process.GetProcessById(process.Id);
                    _ = observer.SafeHandle;
                    terminator.Attach(process);
                }, CancellationToken.None));

            await observer!.WaitForExitAsync().WaitAsync(TimeSpan.FromSeconds(5));
            await Assert.That(File.Exists(marker)).IsFalse();
        }
        finally
        {
            using (observer)
            {
                if (observer is { HasExited: false })
                {
                    observer.Kill();
                    await observer.WaitForExitAsync().WaitAsync(TimeSpan.FromSeconds(5));
                }
            }
        }
    }

    [Test]
    [RequiresTool("dotnet")]
    public async Task Immediate_Descendant_Is_Killed_By_Command_Cancellation()
    {
        await using var fixture = new ProcessTreeFixture();
        await using var otherCommand = new ProcessTreeFixture();
        var command = await GetService<ICommandContext>();
        // No trigger: the command spawns a descendant as soon as its managed code starts.
        fixture.Start(command, "parent", TimeSpan.Zero);
        var child = await fixture.WaitForReadyProcessAsync("child", TimeSpan.FromSeconds(10));
        otherCommand.Start(command, "parent", TimeSpan.Zero);
        var otherChild = await otherCommand.WaitForReadyProcessAsync("child", TimeSpan.FromSeconds(10));
        fixture.Cancel();

        await Assert.ThrowsAsync<OperationCanceledException>(() => fixture.Execution.WaitAsync(TimeSpan.FromSeconds(5)));
        await child.WaitForExitAsync().WaitAsync(TimeSpan.FromSeconds(5));
        await Assert.That(otherChild.HasExited).IsFalse();
    }
}
