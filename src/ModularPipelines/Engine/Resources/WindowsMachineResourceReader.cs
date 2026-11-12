using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace ModularPipelines.Engine.Resources;

[SupportedOSPlatform("windows")]
internal sealed partial class WindowsMachineResourceReader : IMachineResourceReader
{
    public bool TryRead(out MachineResourceReading reading)
    {
        reading = default;
        var memory = new MemoryStatusEx { Length = (uint) Unsafe.SizeOf<MemoryStatusEx>() };

        // Kernel time includes idle time.
        if (!GetSystemTimes(out var idle, out var kernel, out var user) || !GlobalMemoryStatusEx(ref memory))
        {
            return false;
        }

        reading = new MachineResourceReading(idle, kernel + user, (long) memory.TotalPhysical, (long) memory.AvailablePhysical);
        return true;
    }

    // FILETIME is two 32-bit halves, low first, so it reads directly as a 64-bit tick count.
    [LibraryImport("kernel32.dll")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvStdcall)])]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool GetSystemTimes(out long idleTime, out long kernelTime, out long userTime);

    [LibraryImport("kernel32.dll")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvStdcall)])]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool GlobalMemoryStatusEx(ref MemoryStatusEx buffer);

    [StructLayout(LayoutKind.Sequential)]
    private struct MemoryStatusEx
    {
        public uint Length;
        public uint MemoryLoad;
        public ulong TotalPhysical;
        public ulong AvailablePhysical;
        public ulong TotalPageFile;
        public ulong AvailablePageFile;
        public ulong TotalVirtual;
        public ulong AvailableVirtual;
        public ulong AvailableExtendedVirtual;
    }
}
