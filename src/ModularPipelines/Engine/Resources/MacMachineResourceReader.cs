using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace ModularPipelines.Engine.Resources;

[SupportedOSPlatform("macos")]
internal sealed partial class MacMachineResourceReader : IMachineResourceReader
{
    private const string SystemLibrary = "/usr/lib/libSystem.dylib";
    private const int HostCpuLoadInfo = 3;
    private const int HostVmInfo64 = 4;

    // host_cpu_load_info: user, system, idle, nice ticks.
    private const int CpuLoadInfoCount = 4;

    // vm_statistics64 measured in 32-bit words.
    private const int VmInfo64Count = 38;
    private const int WireCountIndex = 3;
    private const int PurgeableCountIndex = 22;
    private const int CompressorPageCountIndex = 32;
    private const int InternalPageCountIndex = 35;

    private readonly uint _host = GetHostPort();
    private readonly long _totalMemoryBytes = ReadTotalMemoryBytes();

    public unsafe bool TryRead(out MachineResourceReading reading)
    {
        reading = default;
        if (_totalMemoryBytes <= 0)
        {
            return false;
        }

        var cpu = stackalloc uint[CpuLoadInfoCount];
        var cpuCount = (uint) CpuLoadInfoCount;
        var vm = stackalloc uint[VmInfo64Count];
        var vmCount = (uint) VmInfo64Count;
        if (GetHostStatistics(_host, HostCpuLoadInfo, cpu, ref cpuCount) != 0
            || cpuCount < CpuLoadInfoCount
            || GetHostStatistics64(_host, HostVmInfo64, vm, ref vmCount) != 0
            || vmCount <= InternalPageCountIndex)
        {
            return false;
        }

        long idle = cpu[2];
        var total = cpu[0] + (long) cpu[1] + cpu[2] + cpu[3];

        // Matches Activity Monitor's "Memory Used": app memory, wired memory, and compressed memory.
        // File cache and purgeable pages are reclaimable, so they count as available.
        var usedPages = (long) vm[InternalPageCountIndex] - vm[PurgeableCountIndex]
                        + vm[WireCountIndex] + vm[CompressorPageCountIndex];
        var available = Math.Clamp(_totalMemoryBytes - (usedPages * Environment.SystemPageSize), 0, _totalMemoryBytes);

        reading = new MachineResourceReading(idle, total, _totalMemoryBytes, available);
        return true;
    }

    private static unsafe long ReadTotalMemoryBytes()
    {
        long bytes = 0;
        var length = (nuint) sizeof(long);
        return GetSystemControlValue("hw.memsize", &bytes, ref length, null, 0) == 0 ? bytes : 0;
    }

    [LibraryImport(SystemLibrary, EntryPoint = "mach_host_self")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    private static partial uint GetHostPort();

    [LibraryImport(SystemLibrary, EntryPoint = "host_statistics")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    private static unsafe partial int GetHostStatistics(uint host, int flavor, uint* info, ref uint count);

    [LibraryImport(SystemLibrary, EntryPoint = "host_statistics64")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    private static unsafe partial int GetHostStatistics64(uint host, int flavor, uint* info, ref uint count);

    [LibraryImport(SystemLibrary, EntryPoint = "sysctlbyname", StringMarshalling = StringMarshalling.Utf8)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    private static unsafe partial int GetSystemControlValue(string name, void* oldValue, ref nuint oldLength, void* newValue, nuint newLength);
}
