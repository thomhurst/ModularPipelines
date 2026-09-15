using System.Diagnostics;
using System.Text.Json;

namespace ModularPipelines.ProcessTestHost;

public static class Host
{
    public static async Task<int> Main(string[] args)
    {
        Console.CancelKeyPress += (_, eventArgs) => eventArgs.Cancel = true;
        var role = args[0];
        var directory = args[1];
        var runtimeConfiguration = args[2];
        var name = role is "parent-exit" or "grace-parent" or "startup-failure" or "never-ready" or "child-failure-parent" ? "parent" : role;
        if (name == "parent")
        {
            using var currentProcess = Process.GetCurrentProcess();
            Publish(directory, name + ".pid", JsonSerializer.Serialize(ProcessIdentity.Capture(currentProcess)));
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

    private static Process StartChild(string role, string directory, string runtimeConfiguration)
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

        var child = Process.Start(startInfo)!;
        Publish(directory, role + ".pid", JsonSerializer.Serialize(ProcessIdentity.Capture(child)));
        return child;
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
