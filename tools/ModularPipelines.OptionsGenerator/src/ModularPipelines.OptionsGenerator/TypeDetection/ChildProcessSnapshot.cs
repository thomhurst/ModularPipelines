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
        public static LinuxChildProcessSnapshot Instance { get; } = new();

        public IEnumerable<int> GetChildProcessIds(int parentProcessId)
        {
            var childrenPath = $"/proc/{parentProcessId}/task/{parentProcessId}/children";
            if (!File.Exists(childrenPath))
            {
                return [];
            }

            return File.ReadAllText(childrenPath)
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(static value => int.TryParse(value, out var processId) ? processId : 0)
                .Where(static processId => processId > 0)
                .ToArray();
        }
    }

    private sealed class MacOsChildProcessSnapshot : IChildProcessSnapshot
    {
        public static MacOsChildProcessSnapshot Instance { get; } = new();

        public IEnumerable<int> GetChildProcessIds(int parentProcessId)
        {
            var capacity = ProcListChildPids(parentProcessId, nint.Zero, 0);
            if (capacity <= 0)
            {
                return [];
            }

            var buffer = Marshal.AllocHGlobal(checked(capacity * sizeof(int)));
            try
            {
                var count = ProcListChildPids(parentProcessId, buffer, capacity * sizeof(int));
                if (count <= 0)
                {
                    return [];
                }

                var processIds = new int[Math.Min(count, capacity)];
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
