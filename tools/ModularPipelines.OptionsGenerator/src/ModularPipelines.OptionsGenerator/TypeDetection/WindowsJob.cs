using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace ModularPipelines.OptionsGenerator.TypeDetection;

internal static class WindowsJob
{
    private const uint KillOnJobClose = 0x00002000;

    public static SafeFileHandle? TryCreate(bool killOnClose = false)
    {
        var job = CreateJobObject(nint.Zero, null);
        if (job.IsInvalid)
        {
            job.Dispose();
            return null;
        }

        if (!killOnClose)
        {
            return job;
        }

        var information = new JobObjectExtendedLimitInformation
        {
            BasicLimitInformation = new JobObjectBasicLimitInformation
            {
                LimitFlags = KillOnJobClose,
            },
        };
        if (SetInformationJobObject(
                job,
                JobObjectInfoClass.ExtendedLimitInformation,
                ref information,
                (uint) Marshal.SizeOf<JobObjectExtendedLimitInformation>()))
        {
            return job;
        }

        job.Dispose();
        return null;
    }

    public static bool TryAssign(SafeFileHandle job, SafeProcessHandle process) =>
        AssignProcessToJobObject(job, process);

    public static bool TryGetActiveProcessCount(
        SafeFileHandle job,
        out uint activeProcessCount)
    {
        var information = new JobObjectBasicAccountingInformation();
        var succeeded = QueryInformationJobObject(
            job,
            JobObjectInfoClass.BasicAccountingInformation,
            ref information,
            (uint) Marshal.SizeOf<JobObjectBasicAccountingInformation>(),
            out _);
        activeProcessCount = information.ActiveProcesses;
        return succeeded;
    }

#pragma warning disable SYSLIB1054 // LibraryImport requires unsafe blocks, which this project does not enable.
    [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern SafeFileHandle CreateJobObject(nint jobAttributes, string? name);

    [DllImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool SetInformationJobObject(
        SafeFileHandle job,
        JobObjectInfoClass informationClass,
        ref JobObjectExtendedLimitInformation information,
        uint informationLength);

    [DllImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool AssignProcessToJobObject(
        SafeFileHandle job,
        SafeProcessHandle process);

    [DllImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool QueryInformationJobObject(
        SafeFileHandle job,
        JobObjectInfoClass informationClass,
        ref JobObjectBasicAccountingInformation information,
        uint informationLength,
        out uint returnLength);
#pragma warning restore SYSLIB1054

    private enum JobObjectInfoClass
    {
        BasicAccountingInformation = 1,
        ExtendedLimitInformation = 9,
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct JobObjectBasicAccountingInformation
    {
        public long TotalUserTime;
        public long TotalKernelTime;
        public long ThisPeriodTotalUserTime;
        public long ThisPeriodTotalKernelTime;
        public uint TotalPageFaultCount;
        public uint TotalProcesses;
        public uint ActiveProcesses;
        public uint TotalTerminatedProcesses;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct JobObjectBasicLimitInformation
    {
        public long PerProcessUserTimeLimit;
        public long PerJobUserTimeLimit;
        public uint LimitFlags;
        public nuint MinimumWorkingSetSize;
        public nuint MaximumWorkingSetSize;
        public uint ActiveProcessLimit;
        public nuint Affinity;
        public uint PriorityClass;
        public uint SchedulingClass;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct IoCounters
    {
        public ulong ReadOperationCount;
        public ulong WriteOperationCount;
        public ulong OtherOperationCount;
        public ulong ReadTransferCount;
        public ulong WriteTransferCount;
        public ulong OtherTransferCount;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct JobObjectExtendedLimitInformation
    {
        public JobObjectBasicLimitInformation BasicLimitInformation;
        public IoCounters IoInfo;
        public nuint ProcessMemoryLimit;
        public nuint JobMemoryLimit;
        public nuint PeakProcessMemoryUsed;
        public nuint PeakJobMemoryUsed;
    }
}
