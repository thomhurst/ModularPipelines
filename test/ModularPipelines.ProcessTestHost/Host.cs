using System.Diagnostics;
using System.Text.Json;

namespace ModularPipelines.ProcessTestHost;

public static class Host
{
    public static async Task<int> Main(string[] args)
    {
        if (args[0] == "write-output")
        {
            foreach (var line in args.Skip(1))
            {
                Console.WriteLine(line);
            }

            return 0;
        }

        Console.CancelKeyPress += (_, eventArgs) => eventArgs.Cancel = true;
        var role = args[0];
        var directory = args[1];
        var runtimeConfiguration = args[2];
        var name = role is "parent-exit" or "graceful-exit" or "grace-parent" or "startup-failure" or "never-ready" or "child-failure-parent" or "orphaning-parent" ? "parent" : role;
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

            return 0;
        }
        catch (Exception exception)
        {
            Publish(directory, name + ".error", exception.ToString());
            await Console.Error.WriteLineAsync(exception.ToString());
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
            case "orphaning-parent":
                // Simulates a parent killed after starting a child but before publishing its PID.
                CreateChildProcess("child", directory, runtimeConfiguration).Dispose();
                break;
            case "graceful-exit":
                await WaitForTrigger(directory, "parent-exit");
                break;
            case "parent":
            case "parent-exit":
            case "grace-parent":
            case "child-failure-parent":
                var childRole = role switch
                {
                    "grace-parent" => "intermediate",
                    "child-failure-parent" => "failing-child",
                    _ => "child",
                };
                using (var child = StartChild(childRole, directory, runtimeConfiguration))
                {
                    if (role == "parent-exit")
                    {
                        await WaitForTrigger(directory, "parent-exit");
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
            case "child":
            case "grandchild":
                await Task.Delay(TimeSpan.FromSeconds(60));
                break;
            default:
                throw new ArgumentException($"Unknown fixture role: {role}");
        }
    }

    private static Process CreateChildProcess(string role, string directory, string runtimeConfiguration)
    {
        var startInfo = new ProcessStartInfo(Environment.ProcessPath!)
        {
            UseShellExecute = false,
            CreateNoWindow = true,
        };
        foreach (var argument in new[] { "exec", "--runtimeconfig", runtimeConfiguration, typeof(Host).Assembly.Location, role, directory, runtimeConfiguration })
        {
            startInfo.ArgumentList.Add(argument);
        }

        return Process.Start(startInfo)!;
    }

    private static Process StartChild(string role, string directory, string runtimeConfiguration)
    {
        var child = CreateChildProcess(role, directory, runtimeConfiguration);
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
