using System.Diagnostics;
using ModularPipelines.Engine;
using ModularPipelines.ProcessTestHost;
using ModularPipelines.UnitTests.Helpers;

namespace ModularPipelines.UnitTests.Execution;

[TUnit.Core.ParallelLimiter<ProcessFixtureParallelLimit>]
public class ShutdownProcessTests
{
    [Test]
    public Task ProcessExit_Waits_For_Cleanup_Hooks_Report_And_Disposal() => RunShutdownAsync("pipeline-exit");

    [Test]
    public Task ProcessExit_Bounds_Blocking_Cancellation_Callback() => RunShutdownAsync("pipeline-exit-blocked-cancellation");

    [Test]
    [Arguments("pipeline-sigterm-before-run")]
    [Arguments("pipeline-sigterm-after-run")]
    public async Task Sigterm_Without_Active_Run_Allows_Process_Exit(string mode)
    {
        if (OperatingSystem.IsWindows())
        {
            return;
        }

        await RunShutdownAsync(mode);
    }

    [Test]
    public async Task Sigterm_Completes_Cleanup_Hooks_Report_And_Disposal()
    {
        if (OperatingSystem.IsWindows())
        {
            return;
        }

        await RunShutdownAsync("pipeline-sigterm");
    }

    private static async Task RunShutdownAsync(string mode)
    {
        var directory = Directory.CreateTempSubdirectory("modular-pipelines-shutdown-").FullName;
        await File.WriteAllTextAsync(Path.Combine(directory, "events.log"), string.Empty);
        var testAssembly = typeof(ShutdownProcessTests).Assembly.Location;
        using var startupDeadline = new CancellationTokenSource(TimeSpan.FromSeconds(45));
        using var shutdownDeadline = new CancellationTokenSource();
        var phase = "startup";
        var startInfo = new ProcessStartInfo("dotnet")
        {
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            WorkingDirectory = directory,
        };
        foreach (var argument in new[]
                 {
                     "exec", "--runtimeconfig", Path.ChangeExtension(testAssembly, ".runtimeconfig.json"),
                     "--depsfile", Path.ChangeExtension(testAssembly, ".deps.json"),
                     typeof(Host).Assembly.Location, mode, directory,
                 })
        {
            startInfo.ArgumentList.Add(argument);
        }

        using var process = Process.Start(startInfo)!;
        var output = process.StandardOutput.ReadToEndAsync();
        var error = process.StandardError.ReadToEndAsync();
        try
        {
            while (!File.Exists(Path.Combine(directory, "ready")))
            {
                if (process.HasExited)
                {
                    throw new InvalidOperationException($"Shutdown fixture exited before readiness: {await output} {await error}");
                }

                await Task.Delay(20, startupDeadline.Token);
            }

            // Startup and pipeline initialization must not consume the exit assertion's budget.
            phase = "shutdown";
            shutdownDeadline.CancelAfter(TimeSpan.FromSeconds(45));
            if (mode.StartsWith("pipeline-exit", StringComparison.Ordinal))
            {
                await File.WriteAllTextAsync(Path.Combine(directory, "exit.trigger"), string.Empty, shutdownDeadline.Token);
            }
            else
            {
                using var signal = Process.Start(new ProcessStartInfo("kill")
                {
                    ArgumentList = { "-TERM", process.Id.ToString(System.Globalization.CultureInfo.InvariantCulture) },
                    UseShellExecute = false,
                })!;
                await signal.WaitForExitAsync(shutdownDeadline.Token);
                await Assert.That(signal.ExitCode).IsEqualTo(0);
            }

            await process.WaitForExitAsync(shutdownDeadline.Token);
            await AssertShutdownResultAsync(mode, process, directory, shutdownDeadline.Token);
        }
        catch (OperationCanceledException exception) when (
            phase == "startup" ? startupDeadline.IsCancellationRequested : shutdownDeadline.IsCancellationRequested)
        {
            if (!process.HasExited)
            {
                process.Kill();
                await process.WaitForExitAsync();
            }

            var events = await File.ReadAllTextAsync(Path.Combine(directory, "events.log"));
            throw new TimeoutException(
                $"Shutdown fixture {mode} timed out during {phase}. " +
                $"Ready: {File.Exists(Path.Combine(directory, "ready"))}. " +
                $"Events: {events}. Standard output: {await output}. Standard error: {await error}", exception);
        }
        finally
        {
            if (!process.HasExited)
            {
                process.Kill();
                await process.WaitForExitAsync();
            }

            await Task.WhenAll(output, error);
            Directory.Delete(directory, recursive: true);
        }
    }

    private static async Task AssertShutdownResultAsync(string mode, Process process, string directory, CancellationToken cancellationToken)
    {
        if (mode == "pipeline-exit-blocked-cancellation")
        {
            await Assert.That(process.ExitCode).IsEqualTo(0);
            return;
        }

        if (mode is "pipeline-sigterm-before-run" or "pipeline-sigterm-after-run")
        {
            // Since .NET 10, default SIGTERM termination does not raise ProcessExit.
            // Idle hosts must terminate rather than swallowing the signal and waiting forever.
            await Assert.That(process.ExitCode).IsEqualTo(128 + 15);
            var idleEvents = await File.ReadAllLinesAsync(Path.Combine(directory, "events.log"), cancellationToken);
            var expected = mode == "pipeline-sigterm-before-run" ? string.Empty : "cleanup,end";
            await Assert.That(string.Join(",", idleEvents)).IsEqualTo(expected);
            await Assert.That(File.Exists(Path.Combine(directory, "report.json")))
                .IsEqualTo(mode == "pipeline-sigterm-after-run");
            return;
        }

        await Assert.That(process.ExitCode).IsEqualTo(0);
        var events = await File.ReadAllLinesAsync(Path.Combine(directory, "events.log"), cancellationToken);
        await Assert.That(string.Join(",", events)).IsEqualTo("cleanup,end,disposed-after-report");
        var report = RunReportJsonSerializer.Deserialize(await File.ReadAllTextAsync(Path.Combine(directory, "report.json"), cancellationToken));
        await Assert.That(report!.Modules.Single(module => module.Name == "CleanupModule").Status).IsEqualTo(ModuleStatus.Succeeded);
    }
}
