using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace ModularPipelines.Engine.Resources;

internal static class WindowsProcessorTimes
{
    internal delegate bool ReadGroup(ushort group, Span<ProcessorPerformance> buffer, out uint bytesWritten);

    internal static bool TryRead(ushort groupCount, ReadGroup readGroup, out long idle, out long total)
    {
        idle = 0;
        total = 0;
        if (groupCount == 0)
        {
            return false;
        }

        // Windows processor groups contain at most 64 logical processors. Reuse one stack buffer.
        Span<ProcessorPerformance> buffer = stackalloc ProcessorPerformance[64];
        var entrySize = (uint) Unsafe.SizeOf<ProcessorPerformance>();
        long idleSum = 0;
        long totalSum = 0;
        for (ushort group = 0; group < groupCount; group++)
        {
            if (!readGroup(group, buffer, out var bytesWritten)
                || bytesWritten == 0 || bytesWritten > buffer.Length * entrySize || bytesWritten % entrySize != 0)
            {
                // Never publish a partial-machine reading if any group failed.
                return false;
            }

            foreach (var processor in buffer[..((int) bytesWritten / (int) entrySize)])
            {
                idleSum += processor.IdleTime;
                // Kernel time already includes idle time.
                totalSum += processor.KernelTime + processor.UserTime;
            }
        }

        idle = idleSum;
        total = totalSum;
        return true;
    }

    // SYSTEM_PROCESSOR_PERFORMANCE_INFORMATION, including reserved fields and native tail padding.
    [StructLayout(LayoutKind.Sequential, Size = 48)]
    internal struct ProcessorPerformance
    {
        public long IdleTime;
        public long KernelTime;
        public long UserTime;
        private long _reserved1;
        private long _reserved2;
        private uint _reserved3;
    }
}
