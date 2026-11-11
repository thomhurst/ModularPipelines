using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text.Json;
using Microsoft.Win32.SafeHandles;

// Standalone runtime reproducer: no ModularPipelines assembly or package dependencies.
internal static class Program
{
    private static readonly TimeSpan Deadline = TimeSpan.FromSeconds(15);

    public static async Task<int> Main(string[] args)
    {
        if (!OperatingSystem.IsWindows())
        {
            throw new PlatformNotSupportedException("This probe requires Windows job objects.");
        }

        if (args[0] == "leaf")
        {
            WriteIdentity(args[1], "leaf");
            await Task.Delay(Timeout.InfiniteTimeSpan);
            return 0;
        }

        if (args[0] == "worker")
        {
            // Spawn before publishing readiness: the descendant starts immediately, with no
            // opportunity for the parent to assign a job after Process.Start returns.
            using var leaf = Process.Start(StartInfo("leaf", args[1]))!;
            WriteIdentity(args[1], "worker");
            await Task.Delay(Timeout.InfiniteTimeSpan);
            return 0;
        }

        if (args[0] == "supervisor")
        {
            using var firstJob = EnterJob();
            using var secondJob = EnterJob();
            var flags = int.Parse(args[2]);
            var info = StartInfo("worker", args[1]);
            info.KillOnParentExit = (flags & 8) != 0;
            info.RedirectStandardInput = info.RedirectStandardOutput = info.RedirectStandardError = (flags & 1) != 0;
            if ((flags & 2) != 0)
            {
                info.InheritedHandles = [];
            }

            info.CreateNewProcessGroup = (flags & 4) != 0;
            try
            {
                using var worker = Process.Start(info)!;
                File.WriteAllText(Path.Combine(args[1], "started"), worker.Id.ToString());
                await Task.Delay(Timeout.InfiniteTimeSpan);
            }
            catch (Win32Exception exception)
            {
                File.WriteAllText(Path.Combine(args[1], "error.json"), JsonSerializer.Serialize(new
                {
                    exception.NativeErrorCode,
                    exception.Message,
                }));
                return 1;
            }

            return 0;
        }

        var output = Path.GetFullPath(args[0]);
        Directory.CreateDirectory(output);
        var processDirectory = Path.Combine(output, "processes-" + Guid.NewGuid().ToString("N"));
        var results = new List<object>();
        var failed = false;
        // First prove the exact command settings work without parent-exit protection.
        // Then isolate each combination to identify interactions with handle inheritance.
        foreach (var flags in new[] { 7, 8, 9, 10, 11, 12, 13, 14, 15 })
        {
            var directory = Path.Combine(processDirectory, flags.ToString());
            Directory.CreateDirectory(directory);
            using var supervisor = Process.Start(StartInfo("supervisor", directory, flags.ToString()))!;
            Process? worker = null;
            Process? leaf = null;
            try
            {
                await WaitUntil(() => File.Exists(Path.Combine(directory, "error.json")) ||
                    (File.Exists(Path.Combine(directory, "worker.json")) && File.Exists(Path.Combine(directory, "leaf.json"))));
                var errorPath = Path.Combine(directory, "error.json");
                if (File.Exists(errorPath))
                {
                    failed = true;
                    var error = JsonSerializer.Deserialize<JsonElement>(await File.ReadAllTextAsync(errorPath));
                    results.Add(new { flags, started = false, error });
                    Console.WriteLine($"flags={flags}: START FAILED: {error}");
                    continue;
                }

                worker = OpenIdentity(directory, "worker");
                leaf = OpenIdentity(directory, "leaf");
                if (worker.HasExited || leaf.HasExited)
                {
                    throw new InvalidOperationException("Worker and immediate descendant must be alive before supervisor death.");
                }

                // Kill only the supervisor, without tree traversal or managed cleanup.
                supervisor.Kill();
                await supervisor.WaitForExitAsync().WaitAsync(Deadline);
                var protectedByRuntime = (flags & 8) != 0;
                var workerExited = await ExitsWithin(worker, protectedByRuntime ? Deadline : TimeSpan.FromSeconds(1));
                var leafExited = await ExitsWithin(leaf, protectedByRuntime ? Deadline : TimeSpan.FromSeconds(1));
                var passed = workerExited == protectedByRuntime && leafExited == protectedByRuntime;
                failed |= !passed;
                results.Add(new { flags, started = true, workerExited, leafExited, passed });
                Console.WriteLine($"flags={flags}: workerExited={workerExited}, immediateDescendantExited={leafExited}, passed={passed}");
            }
            catch (Exception exception)
            {
                failed = true;
                results.Add(new { flags, failure = exception.ToString() });
                Console.WriteLine($"flags={flags}: {exception}");
            }
            finally
            {
                Kill(supervisor);
                // Read the identities again: a timeout can occur after a child was created
                // but before the coordinator obtained its Process instance.
                worker ??= TryOpenIdentity(directory, "worker");
                leaf ??= TryOpenIdentity(directory, "leaf");
                if (worker is not null)
                {
                    Kill(worker);
                    worker.Dispose();
                }

                if (leaf is not null)
                {
                    Kill(leaf);
                    leaf.Dispose();
                }
            }
        }

        var report = new
        {
            runtime = RuntimeInformation.FrameworkDescription,
            os = RuntimeInformation.OSDescription,
            architecture = RuntimeInformation.ProcessArchitecture.ToString(),
            results,
        };
        await File.WriteAllTextAsync(Path.Combine(output, "report.json"),
            JsonSerializer.Serialize(report, new JsonSerializerOptions { WriteIndented = true }));
        Console.WriteLine(JsonSerializer.Serialize(report));
        return failed ? 1 : 0;
    }

    private static ProcessStartInfo StartInfo(params string[] args)
    {
        var info = new ProcessStartInfo(Environment.ProcessPath!) { UseShellExecute = false, CreateNoWindow = true };
        // Executed as `dotnet probe.dll`; retain the entry assembly for every child.
        info.ArgumentList.Add(typeof(Program).Assembly.Location);
        foreach (var argument in args)
        {
            info.ArgumentList.Add(argument);
        }

        return info;
    }

    private static SafeFileHandle EnterJob()
    {
        var job = CreateJobObjectW(IntPtr.Zero, null);
        if (job.IsInvalid)
        {
            throw new Win32Exception(Marshal.GetLastWin32Error());
        }

        using var current = Process.GetCurrentProcess();
        if (!AssignProcessToJobObject(job, current.Handle) || !IsProcessInJob(current.Handle, job, out var inJob) || !inJob)
        {
            var error = Marshal.GetLastWin32Error();
            job.Dispose();
            throw new Win32Exception(error, "Could not establish nested-job precondition.");
        }

        // No kill-on-close here: only the runtime's job may provide that behavior.
        return job;
    }

    private static void WriteIdentity(string directory, string role)
    {
        using var process = Process.GetCurrentProcess();
        var destination = Path.Combine(directory, role + ".json");
        File.WriteAllText(destination + ".tmp", JsonSerializer.Serialize(new Identity(process.Id, process.StartTime.ToUniversalTime().Ticks)));
        File.Move(destination + ".tmp", destination);
    }

    private static Process OpenIdentity(string directory, string role)
    {
        var identity = JsonSerializer.Deserialize<Identity>(File.ReadAllText(Path.Combine(directory, role + ".json")))!;
        var process = Process.GetProcessById(identity.Id);
        if (process.StartTime.ToUniversalTime().Ticks != identity.StartTicks)
        {
            process.Dispose();
            throw new InvalidOperationException("Process ID was reused.");
        }

        return process;
    }

    private static Process? TryOpenIdentity(string directory, string role)
    {
        try { return OpenIdentity(directory, role); }
        catch (ArgumentException) { return null; }
        catch (FileNotFoundException) { return null; }
        catch (InvalidOperationException) { return null; }
    }

    private static async Task WaitUntil(Func<bool> condition)
    {
        using var timeout = new CancellationTokenSource(Deadline);
        while (!condition())
        {
            await Task.Delay(25, timeout.Token);
        }
    }

    private static async Task<bool> ExitsWithin(Process process, TimeSpan timeout)
    {
        try
        {
            await process.WaitForExitAsync().WaitAsync(timeout);
            return true;
        }
        catch (TimeoutException) { return false; }
    }

    private static void Kill(Process process)
    {
        try
        {
            if (!process.HasExited)
            {
                process.Kill(entireProcessTree: true);
                process.WaitForExit(5000);
            }
        }
        catch (ArgumentException) { }
        catch (InvalidOperationException) { }
    }

    private sealed record Identity(int Id, long StartTicks);

    [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern SafeFileHandle CreateJobObjectW(IntPtr attributes, string? name);

    [DllImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool AssignProcessToJobObject(SafeFileHandle job, IntPtr process);

    [DllImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool IsProcessInJob(IntPtr process, SafeFileHandle job, [MarshalAs(UnmanagedType.Bool)] out bool result);
}
