using ModularPipelines.Engine.Resources;

namespace ModularPipelines.UnitTests.Engine.Resources;

public class LinuxMachineResourceReaderTests
{
    [Test]
    [Arguments("/", "/", "/sys/fs/cgroup")]
    [Arguments("/jobs/build", "/", "/sys/fs/cgroup/jobs/build")]
    [Arguments("/jobs/build", "/jobs", "/sys/fs/cgroup/build")]
    [Arguments("/", "/jobs/build", "/sys/fs/cgroup")]
    public async Task ReadsContainerMemoryAndThrottling(string membership, string mountRoot, string directory)
    {
        var files = HostFiles();
        files["/proc/self/cgroup"] = $"0::{membership}\n";
        files["/proc/self/mountinfo"] = $"29 23 0:26 {mountRoot} /sys/fs/cgroup rw - cgroup2 cgroup rw\n";
        files[$"{directory}/memory.max"] = "1000\n";
        files[$"{directory}/memory.current"] = "950\n";
        files[$"{directory}/cpu.stat"] = "usage_usec 9999\nnr_throttled 12\nthrottled_usec 345\n";
        files[$"{directory}/cpu.pressure"] = "some avg10=0.10 avg60=0.00 avg300=0.00 total=123\nfull avg10=0.00 total=1\n";
        files[$"{directory}/memory.pressure"] = "some avg10=0.00 total=456\n";

        var reader = new LinuxMachineResourceReader(path => files.GetValueOrDefault(path));
        await Assert.That(reader.TryRead(out var reading)).IsTrue();
        await Assert.That(reading.MemoryTotalBytes).IsEqualTo(1000);
        await Assert.That(reading.MemoryAvailableBytes).IsEqualTo(50);
        await Assert.That(reading.Pressure!.ThrottledPeriods).IsEqualTo(12);
        await Assert.That(reading.Pressure.ThrottledMicroseconds).IsEqualTo(345);
        await Assert.That(reading.Pressure.CpuStallMicroseconds).IsEqualTo(123);
        await Assert.That(reading.Pressure.MemoryStallMicroseconds).IsEqualTo(456);
    }

    [Test]
    public async Task UsesVisibleParentLimitAndItsUsage_WhenLeafIsUnlimited()
    {
        var files = CgroupFiles();
        files["/sys/fs/cgroup/job/memory.max"] = "max\n";
        files["/sys/fs/cgroup/memory.max"] = "1000\n";
        files["/sys/fs/cgroup/memory.current"] = "1200\n";
        var reader = new LinuxMachineResourceReader(path => files.GetValueOrDefault(path));

        await Assert.That(reader.TryRead(out var reading)).IsTrue();
        await Assert.That(reading.MemoryTotalBytes).IsEqualTo(1000);
        await Assert.That(reading.MemoryAvailableBytes).IsEqualTo(0);
    }

    [Test]
    [Arguments("max", "100")]
    [Arguments("0", "100")]
    [Arguments("-1", "100")]
    [Arguments("1000", "bad")]
    [Arguments("bad", "100")]
    [Arguments("999999999999999999999999", "100")]
    [Arguments("999999999", "100")]
    public async Task InvalidOrUnlimitedMemoryFallsBackToHost(string maximum, string current)
    {
        var files = CgroupFiles();
        files["/sys/fs/cgroup/job/memory.max"] = maximum;
        files["/sys/fs/cgroup/job/memory.current"] = current;
        var reader = new LinuxMachineResourceReader(path => files.GetValueOrDefault(path));

        await Assert.That(reader.TryRead(out var reading)).IsTrue();
        await Assert.That(reading.MemoryTotalBytes).IsEqualTo(10000 * 1024);
        await Assert.That(reading.MemoryAvailableBytes).IsEqualTo(5000 * 1024);
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task UnavailableOptionalFilesDoNotDisableMonitoring(bool denied)
    {
        var files = HostFiles();
        files["/proc/pressure/cpu"] = "some avg10=0.00 total=123\n";
        files["/proc/pressure/memory"] = "some avg10=0.00 total=456\n";
        var reader = new LinuxMachineResourceReader(path => files.TryGetValue(path, out var value)
            ? value : throw (denied ? new UnauthorizedAccessException() : new IOException()));

        await Assert.That(reader.TryRead(out var reading)).IsTrue();
        await Assert.That(reading.Pressure!.CpuStallMicroseconds).IsEqualTo(123);
        await Assert.That(reading.Pressure.MemoryStallMicroseconds).IsEqualTo(456);
        await Assert.That(reading.Pressure.ThrottledPeriods).IsNull();
    }

    [Test]
    public async Task MalformedOptionalCountersAreIgnored()
    {
        var files = CgroupFiles();
        files["/sys/fs/cgroup/job/cpu.stat"] = "nr_throttled -1\nthrottled_usec invalid\n";
        files["/sys/fs/cgroup/job/cpu.pressure"] = "some avg10=1.00 total=-2\n";
        files["/sys/fs/cgroup/job/memory.pressure"] = "full avg10=1.00 total=100\n";
        var reader = new LinuxMachineResourceReader(path => files.GetValueOrDefault(path));

        await Assert.That(reader.TryRead(out var reading)).IsTrue();
        await Assert.That(reading.Pressure!.ThrottledPeriods).IsNull();
        await Assert.That(reading.Pressure.ThrottledMicroseconds).IsNull();
        await Assert.That(reading.Pressure.CpuStallMicroseconds).IsNull();
        await Assert.That(reading.Pressure.MemoryStallMicroseconds).IsNull();
    }

    [Test]
    public async Task ResolvesEscapedMountPoint()
    {
        var files = HostFiles();
        files["/proc/self/cgroup"] = "0::/\n";
        files["/proc/self/mountinfo"] = "29 23 0:26 / /cgroup\\040mount rw - cgroup2 cgroup rw\n";
        files["/cgroup mount/memory.max"] = "1000";
        files["/cgroup mount/memory.current"] = "750";
        var reader = new LinuxMachineResourceReader(path => files.GetValueOrDefault(path));

        await Assert.That(reader.TryRead(out var reading)).IsTrue();
        await Assert.That(reading.MemoryAvailableBytes).IsEqualTo(250);
    }

    private static Dictionary<string, string> HostFiles() => new()
    {
        ["/proc/stat"] = "cpu 100 0 0 900 0 0 0 0\n",
        ["/proc/meminfo"] = "MemTotal: 10000 kB\nMemAvailable: 5000 kB\n",
    };

    private static Dictionary<string, string> CgroupFiles()
    {
        var files = HostFiles();
        files["/proc/self/cgroup"] = "0::/job\n";
        files["/proc/self/mountinfo"] = "29 23 0:26 / /sys/fs/cgroup rw - cgroup2 cgroup rw\n";
        return files;
    }
}
