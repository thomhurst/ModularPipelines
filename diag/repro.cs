using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

var mode = args[0];
for (var i = 1; i <= 200; i++)
{
    using var p = Process.Start(new ProcessStartInfo("/bin/sh", ["-c", "sleep 60 & sleep 60; wait"]) { RedirectStandardOutput = true })!;
    Thread.Sleep(300);
    Console.WriteLine($"{DateTime.UtcNow:HH:mm:ss.fff} iteration {i}: {mode} kill of {p.Id}");
    if (mode == "runtime") p.Kill(entireProcessTree: true); else ProcessTree.Kill(p);
    p.WaitForExit();
}
Console.WriteLine("completed 200 iterations");



/// <summary>
/// Finds and kills process trees without <c>Process.Kill(entireProcessTree: true)</c> on macOS.
/// </summary>
/// <remarks>
/// On GitHub-hosted macOS arm64 runners, the runtime's recursive stop-then-kill can freeze every
/// process in the job, including the runner, which then loses contact with GitHub
/// (dotnet/runtime#131944, actions/runner-images#14833). Killing each process individually from a
/// descendant snapshot does not trigger it.
/// </remarks>
static class ProcessTree
{
    /// <summary>
    /// Kills a process and every descendant found when the call starts.
    /// </summary>
    /// <param name="process">The root of the process tree.</param>
    public static void Kill(Process process)
    {
        if (!OperatingSystem.IsMacOS())
        {
            process.Kill(entireProcessTree: true);
            return;
        }

        // Snapshot first: once the root dies, its children are reparented to launchd.
        var descendantProcessIds = GetDescendantProcessIds(process.Id);
        process.Kill();

        foreach (var descendantProcessId in descendantProcessIds)
        {
            TryKill(descendantProcessId);
        }
    }

    /// <summary>
    /// Returns the IDs of every current descendant of a process. Returns an empty set on Windows.
    /// </summary>
    /// <param name="rootProcessId">The process whose descendants to find.</param>
    public static HashSet<int> GetDescendantProcessIds(int rootProcessId)
    {
        var pendingProcessIds = new Queue<int>();
        var descendantProcessIds = new HashSet<int>();
        pendingProcessIds.Enqueue(rootProcessId);

        while (pendingProcessIds.TryDequeue(out var parentProcessId))
        {
            foreach (var childProcessId in GetChildProcessIds(parentProcessId))
            {
                if (descendantProcessIds.Add(childProcessId))
                {
                    pendingProcessIds.Enqueue(childProcessId);
                }
            }
        }

        return descendantProcessIds;
    }

    private static int[] GetChildProcessIds(int parentProcessId)
    {
        if (OperatingSystem.IsLinux())
        {
            var childrenFile = $"/proc/{parentProcessId}/task/{parentProcessId}/children";

            try
            {
                var processIds = File.ReadAllText(childrenFile)
                    .Split((char[]?) null, StringSplitOptions.RemoveEmptyEntries);
                return Array.ConvertAll(processIds, int.Parse);
            }
            catch (IOException)
            {
                return [];
            }
            catch (UnauthorizedAccessException)
            {
                return [];
            }
        }

        if (OperatingSystem.IsMacOS())
        {
            return MacNativeMethods.GetChildProcessIds(parentProcessId);
        }

        return [];
    }

    private static void TryKill(int processId)
    {
        try
        {
            using var process = Process.GetProcessById(processId);
            process.Kill();
        }
        catch (ArgumentException)
        {
            // The process already exited.
        }
        catch (InvalidOperationException)
        {
            // The process already exited.
        }
        catch (Win32Exception)
        {
            // The process exited or could not be terminated.
        }
    }

    [SupportedOSPlatform("macos")]
    private static class MacNativeMethods
    {
        public static int[] GetChildProcessIds(int parentProcessId)
        {
            var processCount = ProcListChildPids(parentProcessId, null, 0);
            if (processCount <= 0)
            {
                return [];
            }

            var processIds = new int[processCount];
            var actualProcessCount =
                ProcListChildPids(parentProcessId, processIds, processIds.Length * sizeof(int));
            return actualProcessCount > 0
                ? processIds[..Math.Min(actualProcessCount, processIds.Length)]
                : [];
        }

#pragma warning disable SYSLIB1054 // LibraryImport requires unsafe blocks, which this project does not enable.
        [DllImport("libproc", EntryPoint = "proc_listchildpids")]
        private static extern int ProcListChildPids(
            int parentProcessId,
            int[]? processIds,
            int byteCount);
#pragma warning restore SYSLIB1054
    }
}
