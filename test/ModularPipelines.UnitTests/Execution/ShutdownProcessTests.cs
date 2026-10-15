using System.Diagnostics;
using ModularPipelines.Engine;
using ModularPipelines.Enums;
using ModularPipelines.ProcessTestHost;

namespace ModularPipelines.UnitTests.Execution;

public class ShutdownProcessTests
{
    [Test]
    public Task ProcessExit_Waits_For_Cleanup_Hooks_Report_And_Disposal() => RunShutdownAsync("pipeline-exit");

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
        var testAssembly = typeof(ShutdownProcessTests).Assembly.Location;
        using var deadline = new CancellationTokenSource(TimeSpan.FromSeconds(45));
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

                await Task.Delay(20, deadline.Token);
            }

            if (mode == "pipeline-exit")
            {
                await File.WriteAllTextAsync(Path.Combine(directory, "exit.trigger"), string.Empty, deadline.Token);
            }
            else
            {
                using var signal = Process.Start(new ProcessStartInfo("kill")
                {
                    ArgumentList = { "-TERM", process.Id.ToString(System.Globalization.CultureInfo.InvariantCulture) },
                    UseShellExecute = false,
                })!;
                await signal.WaitForExitAsync(deadline.Token);
                await Assert.That(signal.ExitCode).IsEqualTo(0);
            }

            await process.WaitForExitAsync(deadline.Token);
            await Assert.That(process.ExitCode).IsEqualTo(0);
            var events = await File.ReadAllLinesAsync(Path.Combine(directory, "events.log"), deadline.Token);
            await Assert.That(string.Join(",", events)).IsEqualTo("cleanup,end,disposed-after-report");
            var report = RunReportJsonSerializer.Deserialize(await File.ReadAllTextAsync(Path.Combine(directory, "report.json"), deadline.Token));
            await Assert.That(report!.Modules.Single(module => module.ModuleName == "CleanupModule").Status).IsEqualTo(ModuleStatus.Succeeded);
        }
        finally
        {
            if (!process.HasExited)
            {
                process.Kill(entireProcessTree: true);
                await process.WaitForExitAsync();
            }

            await Task.WhenAll(output, error);
            Directory.Delete(directory, recursive: true);
        }
    }
}
