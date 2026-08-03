using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace ModularPipelines.OptionsGenerator.TypeDetection;

internal interface IChildProcessSnapshot
{
    IEnumerable<int> GetChildProcessIds(int parentProcessId);
}

internal static class ChildProcessSnapshot
{
    public static bool IsSupported =>
        OperatingSystem.IsLinux()
        || OperatingSystem.IsMacOS()
        || OperatingSystem.IsWindows();

    public static IChildProcessSnapshot Create()
    {
        if (OperatingSystem.IsLinux())
        {
            return LinuxChildProcessSnapshot.Instance;
        }

        if (OperatingSystem.IsMacOS())
        {
            return MacOsChildProcessSnapshot.Instance;
        }

        return OperatingSystem.IsWindows()
            ? new WindowsChildProcessSnapshot()
            : EmptyChildProcessSnapshot.Instance;
    }

    private sealed class EmptyChildProcessSnapshot : IChildProcessSnapshot
    {
        public static EmptyChildProcessSnapshot Instance { get; } = new();

        public IEnumerable<int> GetChildProcessIds(int parentProcessId) => [];
    }

    private sealed class LinuxChildProcessSnapshot : IChildProcessSnapshot
    {
        private const long FallbackCacheMilliseconds = 20;
        private static readonly Lock FallbackLock = new();
        private static readonly bool SupportsProcChildren = File.Exists(
            $"/proc/{Environment.ProcessId}/task/{Environment.ProcessId}/children");
        private static ILookup<int, int> _fallbackChildren =
            Array.Empty<(int ProcessId, int ParentProcessId)>()
                .ToLookup(static process => process.ParentProcessId, static process => process.ProcessId);
        private static long _fallbackCapturedAt = long.MinValue;

        public static LinuxChildProcessSnapshot Instance { get; } = new();

        public IEnumerable<int> GetChildProcessIds(int parentProcessId)
        {
            return SupportsProcChildren
                ? GetTargetedChildProcessIds(parentProcessId)
                : GetFallbackChildProcessIds(parentProcessId);
        }

        private static IEnumerable<int> GetTargetedChildProcessIds(int parentProcessId)
        {
            var taskPath = $"/proc/{parentProcessId}/task";
            if (!Directory.Exists(taskPath))
            {
                return [];
            }

            var processIds = new HashSet<int>();
            foreach (var threadPath in Directory.EnumerateDirectories(taskPath))
            {
                var childrenPath = Path.Combine(threadPath, "children");
                if (!File.Exists(childrenPath))
                {
                    continue;
                }

                foreach (var value in File.ReadAllText(childrenPath)
                             .Split(' ', StringSplitOptions.RemoveEmptyEntries))
                {
                    if (int.TryParse(value, out var processId) && processId > 0)
                    {
                        processIds.Add(processId);
                    }
                }
            }

            return processIds;
        }

        private static IEnumerable<int> GetFallbackChildProcessIds(int parentProcessId)
        {
            lock (FallbackLock)
            {
                var now = Environment.TickCount64;
                if (_fallbackCapturedAt == long.MinValue
                    || now - _fallbackCapturedAt >= FallbackCacheMilliseconds)
                {
                    _fallbackChildren = CaptureLinuxProcesses()
                        .ToLookup(
                            static process => process.ParentProcessId,
                            static process => process.ProcessId);
                    _fallbackCapturedAt = now;
                }

                return _fallbackChildren[parentProcessId].ToArray();
            }
        }

        private static IEnumerable<(int ProcessId, int ParentProcessId)> CaptureLinuxProcesses()
        {
            foreach (var processPath in Directory.EnumerateDirectories("/proc"))
            {
                if (int.TryParse(Path.GetFileName(processPath), out var processId)
                    && TryGetParentProcessId(processPath, out var parentProcessId))
                {
                    yield return (processId, parentProcessId);
                }
            }
        }

        private static bool TryGetParentProcessId(
            string processPath,
            out int parentProcessId)
        {
            parentProcessId = default;
            string stat;
            try
            {
                stat = File.ReadAllText(Path.Combine(processPath, "stat"));
            }
            catch (Exception exception) when (exception is
                IOException or UnauthorizedAccessException)
            {
                return false;
            }

            var commandEnd = stat.LastIndexOf(')');
            if (commandEnd < 0)
            {
                return false;
            }

            var fields = stat[(commandEnd + 1)..]
                .Split(' ', StringSplitOptions.RemoveEmptyEntries);
            return fields.Length > 1 && int.TryParse(fields[1], out parentProcessId);
        }
    }

    private sealed class MacOsChildProcessSnapshot : IChildProcessSnapshot
    {
        public static MacOsChildProcessSnapshot Instance { get; } = new();

        public IEnumerable<int> GetChildProcessIds(int parentProcessId)
        {
            // Unlike proc_listpids, proc_listchildpids returns a PID count, not bytes.
            var pidCapacity = ProcListChildPids(parentProcessId, nint.Zero, 0);
            if (pidCapacity <= 0)
            {
                return [];
            }

            var bufferSize = checked(pidCapacity * sizeof(int));
            var buffer = Marshal.AllocHGlobal(bufferSize);
            try
            {
                var pidCount = ProcListChildPids(parentProcessId, buffer, bufferSize);
                if (pidCount <= 0)
                {
                    return [];
                }

                var processIds = new int[Math.Min(pidCount, pidCapacity)];
                Marshal.Copy(buffer, processIds, 0, processIds.Length);
                return processIds.Where(static processId => processId > 0).ToArray();
            }
            finally
            {
                Marshal.FreeHGlobal(buffer);
            }
        }

        [DllImport("/usr/lib/libproc.dylib", EntryPoint = "proc_listchildpids")]
        private static extern int ProcListChildPids(
            int parentProcessId,
            nint buffer,
            int bufferSize);
    }

    private sealed class WindowsChildProcessSnapshot : IChildProcessSnapshot
    {
        private const uint SnapshotProcesses = 0x00000002;
        private readonly ILookup<int, int> _childrenByParent = CaptureProcesses()
            .ToLookup(static process => process.ParentProcessId, static process => process.ProcessId);

        public IEnumerable<int> GetChildProcessIds(int parentProcessId) =>
            _childrenByParent[parentProcessId];

        private static IReadOnlyList<(int ProcessId, int ParentProcessId)> CaptureProcesses()
        {
            var processes = new List<(int ProcessId, int ParentProcessId)>();
            using var snapshot = CreateToolhelp32Snapshot(SnapshotProcesses, 0);
            if (snapshot.IsInvalid)
            {
                return processes;
            }

            var entry = new ProcessEntry32
            {
                Size = (uint) Marshal.SizeOf<ProcessEntry32>(),
                ExecutableFile = string.Empty,
            };
            if (!Process32First(snapshot, ref entry))
            {
                return processes;
            }

            do
            {
                processes.Add(((int) entry.ProcessId, (int) entry.ParentProcessId));
            }
            while (Process32Next(snapshot, ref entry));

            return processes;
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern SafeFileHandle CreateToolhelp32Snapshot(
            uint flags,
            uint processId);

        [DllImport("kernel32.dll", EntryPoint = "Process32FirstW", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool Process32First(
            SafeFileHandle snapshot,
            ref ProcessEntry32 entry);

        [DllImport("kernel32.dll", EntryPoint = "Process32NextW", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool Process32Next(
            SafeFileHandle snapshot,
            ref ProcessEntry32 entry);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        private struct ProcessEntry32
        {
            public uint Size;
            public uint Usage;
            public uint ProcessId;
            public nint DefaultHeapId;
            public uint ModuleId;
            public uint Threads;
            public uint ParentProcessId;
            public int BasePriority;
            public uint Flags;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string ExecutableFile;
        }
    }
}
