using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using Microsoft.Win32.SafeHandles;

namespace ModularPipelines.ProcessTestHost;

// These jobs deliberately have no kill-on-close limit. Only the runtime and pipeline
// jobs may provide the lifetime guarantees asserted by the process fixtures.
[SupportedOSPlatform("windows")]
internal sealed class WindowsContainment : IDisposable
{
    private readonly List<SafeFileHandle> _jobs = [];

    public WindowsContainment()
    {
        try
        {
            using var process = Process.GetCurrentProcess();
            for (var index = 0; index < 2; index++)
            {
                var job = CreateJobObjectW(nint.Zero, null);
                _jobs.Add(job);
                if (job.IsInvalid || !AssignProcessToJobObject(job, process.SafeHandle))
                {
                    throw new Win32Exception(Marshal.GetLastPInvokeError());
                }
            }
        }
        catch
        {
            Dispose();
            throw;
        }
    }

    public void Dispose()
    {
        foreach (var job in _jobs)
        {
            job.Dispose();
        }
    }

    [DllImport("kernel32.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
    private static extern SafeFileHandle CreateJobObjectW(nint attributes, string? name);

    [DllImport("kernel32.dll", ExactSpelling = true, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool AssignProcessToJobObject(SafeFileHandle job, SafeProcessHandle process);
}
