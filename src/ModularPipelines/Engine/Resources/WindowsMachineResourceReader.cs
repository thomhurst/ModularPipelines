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

        if (!WindowsProcessorTimes.TryRead(GetActiveProcessorGroupCount(), ReadProcessorGroup, out var idle, out var total)
            || !GlobalMemoryStatusEx(ref memory))
        {
            return false;
        }

        reading = new MachineResourceReading(idle, total, (long) memory.TotalPhysical, (long) memory.AvailablePhysical);
        return true;
    }

    private static unsafe bool ReadProcessorGroup(ushort group, Span<WindowsProcessorTimes.ProcessorPerformance> buffer, out uint bytesWritten)
    {
        // SystemProcessorPerformanceInformation (8), with an explicit USHORT group selector.
        // Unlike GetSystemTimes, this does not depend on the calling thread's primary group.
        fixed (WindowsProcessorTimes.ProcessorPerformance* processors = buffer)
        {
            return NtQuerySystemInformationEx(8, ref group, sizeof(ushort), processors,
                (uint) buffer.Length * (uint) sizeof(WindowsProcessorTimes.ProcessorPerformance), out bytesWritten) == 0;
        }
    }

    [LibraryImport("kernel32.dll")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvStdcall)])]
    private static partial ushort GetActiveProcessorGroupCount();

    [LibraryImport("ntdll.dll")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvStdcall)])]
    private static unsafe partial int NtQuerySystemInformationEx(int informationClass, ref ushort group,
        uint inputLength, WindowsProcessorTimes.ProcessorPerformance* buffer, uint bufferLength, out uint returnLength);

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
