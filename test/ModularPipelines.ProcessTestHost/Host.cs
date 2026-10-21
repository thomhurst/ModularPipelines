using System.Diagnostics;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Context;
using ModularPipelines.Extensions;
using ModularPipelines.Modules;
using ModularPipelines.Options;

namespace ModularPipelines.ProcessTestHost;

public static class Host
{
    public static async Task<int> Main(string[] args)
    {
        if (args[0] == "stdin-echo")
        {
            System.Console.OutputEncoding = new System.Text.UTF8Encoding(false);
            var prefix = new string('x', int.Parse(args[1]));
            await System.Console.Out.WriteAsync(prefix);
            await System.Console.Error.WriteAsync(prefix);
            using var reader = new StreamReader(System.Console.OpenStandardInput(), System.Text.Encoding.UTF8);
            var input = await reader.ReadToEndAsync();
            await System.Console.Out.WriteAsync(input);
            await System.Console.Error.WriteAsync(input);
            return int.Parse(args[2]);
        }

        if (args[0] == "exit-code")
        {
            return int.Parse(args[1]);
        }

        if (args[0] == "stdin-block")
        {
            await Task.Delay(TimeSpan.FromSeconds(30));
            return 0;
        }

        if (args[0] == "write-bytes")
        {
            using var standardOutput = System.Console.OpenStandardOutput();
            using var standardError = System.Console.OpenStandardError();
            await standardOutput.WriteAsync(Convert.FromBase64String(args[1]));
            await standardError.WriteAsync(Convert.FromBase64String(args[2]));
            return 0;
        }

        if (args[0] is "pipeline-sigterm" or "pipeline-exit" or "pipeline-exit-blocked-cancellation"
            or "pipeline-sigterm-before-run" or "pipeline-sigterm-after-run")
        {
            return await ShutdownPipeline.RunAsync(args[0], args[1]);
        }

        if (args[0] == "write-output")
        {
            foreach (var line in args.Skip(1))
            {
                System.Console.WriteLine(line);
            }

            return 0;
        }

        if (args[0] is "pipeline-host" or "pipeline-background-host")
        {
            return await RunPipelineHostAsync(args);
        }

        return await RunProcessRoleAsync(args);
    }

    private static async Task<int> RunProcessRoleAsync(string[] args)
    {
        System.Console.CancelKeyPress += (_, eventArgs) => eventArgs.Cancel = true;
        var role = args[0];
        var directory = args[1];
        var runtimeConfiguration = args[2];
        var name = role is "threaded-parent" or "parent-error-exit" or "draining-parent" or "parent-exit" or "graceful-exit" or "grace-parent" or "startup-failure" or "never-ready" or "child-failure-parent" or "orphaning-parent" or "background-parent" ? "parent" : role;
        // Every fixture process publishes its own identity. A parent killed between starting a
        // child and publishing its PID would otherwise leave an orphan that cleanup cannot find,
        // holding the command's output pipes open.
        using (var currentProcess = Process.GetCurrentProcess())
        {
            Publish(directory, name == "parent" ? "parent.pid" : name + ".self.pid", JsonSerializer.Serialize(ProcessIdentity.Capture(currentProcess)));
        }
        try
        {
            await RunRoleAsync(role, name, directory, runtimeConfiguration);

            return role == "parent-error-exit" ? 23 : 0;
        }
        catch (Exception exception)
        {
            Publish(directory, name + ".error", exception.ToString());
            await System.Console.Error.WriteLineAsync(exception.ToString());
            return 1;
        }
    }

    private static async Task RunRoleAsync(string role, string name, string directory, string runtimeConfiguration)
    {
        if (role is "startup-failure" or "failing-child")
        {
            throw new InvalidOperationException("Requested process-fixture startup failure.");
        }

        if (role == "never-ready")
        {
            using var child = StartChild("child", directory, runtimeConfiguration);
            await child.WaitForExitAsync();
            return;
        }

        Publish(directory, name + ".ready", "ready");
        switch (role)
        {
            case "background-parent":
                using (var backgroundChild = StartChild("child", directory, runtimeConfiguration, redirectOutput: true))
                {
                    await WaitForPublishedFile(directory, "child.ready");
                }

                break;
            case "threaded-parent":
                // Keep the spawning thread alive so Linux retains the child under that thread's task.
                await Task.Factory.StartNew(() =>
                {
                    using var child = StartChild("child", directory, runtimeConfiguration);
                    child.WaitForExit();
                }, CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Default);
                break;
            case "orphaning-parent":
                // Simulates a parent killed after starting a child but before publishing its PID.
                CreateChildProcess("child", directory, runtimeConfiguration).Dispose();
                await WaitForPublishedFile(directory, "child.ready");
                break;
            case "graceful-exit":
                await WaitForTrigger(directory, "parent-exit");
                break;
            case "parent":
            case "parent-exit":
            case "parent-error-exit":
            case "draining-parent":
            case "grace-parent":
            case "child-failure-parent":
                var childRole = role switch
                {
                    "draining-parent" => "draining-child",
                    "grace-parent" => "intermediate",
                    "child-failure-parent" => "failing-child",
                    _ => "child",
                };
                if (role == "grace-parent")
                {
                    // The grace-period regression starts descendants only after command containment is installed.
                    await WaitForTrigger(directory, "start-child");
                }

                using (var child = StartChild(childRole, directory, runtimeConfiguration))
                {
                    if (role is "parent-exit" or "parent-error-exit" or "draining-parent")
                    {
                        await WaitForTrigger(directory, "parent-exit");
                        System.Console.Out.Write("parent output");
                        System.Console.Error.Write("parent error");
                    }
                    else
                    {
                        await child.WaitForExitAsync();
                        if (child.ExitCode != 0)
                        {
                            throw new InvalidOperationException($"Fixture child {childRole} exited with code {child.ExitCode}.");
                        }
                    }
                }

                break;
            case "intermediate":
                await WaitForTrigger(directory, "spawn");
                using (StartChild("grandchild", directory, runtimeConfiguration))
                {
                    // Preserve the original fixture's exit during the graceful interval.
                    await Task.Delay(TimeSpan.FromMilliseconds(500));
                }

                break;
            case "draining-child":
                await WaitForTrigger(directory, "child-exit");
                System.Console.Out.Write("child output");
                System.Console.Error.Write("child error");
                break;
            case "child":
            case "grandchild":
                await Task.Delay(TimeSpan.FromSeconds(60));
                break;
            default:
                throw new ArgumentException($"Unknown fixture role: {role}");
        }
    }

    private static async Task<int> RunPipelineHostAsync(string[] args)
    {
        var directory = args[1];
        using (var host = Process.GetCurrentProcess())
        {
            Publish(directory, "host.pid", JsonSerializer.Serialize(ProcessIdentity.Capture(host)));
        }

        try
        {
            var builder = Pipeline.CreateBuilder([], sourceFilePath: string.Empty);
            builder.AddModule<IdleModule>();
            await using var pipeline = await builder.BuildAsync();
            var command = pipeline.Services.GetRequiredService<ICommandContext>();
            var childRole = args[0] == "pipeline-background-host" ? "background-parent" : "parent";
            await command.ExecuteCommandLineToolAsync(new CommandLineToolOptions(Environment.ProcessPath!)
            {
                Arguments = ["exec", "--runtimeconfig", args[2], typeof(Host).Assembly.Location, childRole, directory, args[2]],
            });
            Publish(directory, "command-completed.ready", "ready");
            await WaitForTrigger(directory, "dispose-pipeline");
            await pipeline.DisposeAsync();
            Publish(directory, "pipeline-disposed.ready", "ready");
            await WaitForTrigger(directory, "host-exit");
            return 0;
        }
        catch (Exception exception)
        {
            Publish(directory, "host.error", exception.ToString());
            return 1;
        }
    }

    private sealed class IdleModule : Module
    {
        protected override Task ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) => Task.CompletedTask;
    }

    private static async Task WaitForPublishedFile(string directory, string name)
    {
        while (!File.Exists(Path.Combine(directory, name)))
        {
            await Task.Delay(10);
        }
    }

    private static Process CreateChildProcess(string role, string directory, string runtimeConfiguration, bool redirectOutput = false)
    {
        var startInfo = new ProcessStartInfo(Environment.ProcessPath!)
        {
            UseShellExecute = false,
            RedirectStandardOutput = redirectOutput,
            RedirectStandardError = redirectOutput,
            CreateNoWindow = true,
        };
        if (redirectOutput && OperatingSystem.IsWindows())
        {
            // A background child must not inherit the command's original output pipe handles.
            startInfo.InheritedHandles = [];
        }

        foreach (var argument in new[] { "exec", "--runtimeconfig", runtimeConfiguration, typeof(Host).Assembly.Location, role, directory, runtimeConfiguration })
        {
            startInfo.ArgumentList.Add(argument);
        }

        return Process.Start(startInfo)!;
    }

    private static Process StartChild(string role, string directory, string runtimeConfiguration, bool redirectOutput = false)
    {
        var child = CreateChildProcess(role, directory, runtimeConfiguration, redirectOutput);
        try
        {
            Publish(directory, role + ".pid", JsonSerializer.Serialize(ProcessIdentity.Capture(child)));
            return child;
        }
        catch (Exception publicationException)
        {
            using (child)
            {
                try
                {
                    if (!child.HasExited)
                    {
                        // The child has not started any processes yet. A tree kill can freeze
                        // macOS arm64 runners (dotnet/runtime#131944).
                        child.Kill();
                    }

                    if (!child.WaitForExit(5000))
                    {
                        throw new TimeoutException("Unpublished fixture child did not exit after termination.");
                    }
                }
                catch (Exception cleanupException)
                {
                    throw new AggregateException("Fixture child publication and cleanup failed.", publicationException, cleanupException);
                }
            }

            throw;
        }
    }

    private static async Task WaitForTrigger(string directory, string name)
    {
        while (!File.Exists(Path.Combine(directory, name + ".trigger")))
        {
            await Task.Delay(10);
        }
    }

    private static void Publish(string directory, string name, string value)
    {
        // Pollers only see complete files; the spawning process publishes each child's PID.
        var temporaryPath = Path.Combine(directory, $"{name}.{Environment.ProcessId}.tmp");
        File.WriteAllText(temporaryPath, value);
        File.Move(temporaryPath, Path.Combine(directory, name), overwrite: true);
    }
}
