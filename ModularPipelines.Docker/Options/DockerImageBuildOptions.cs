using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("image build")]
public record DockerImageBuildOptions : DockerOptions
{
    [CommandLongSwitch("add-host")]
    public string? AddHost { get; set; }

    [CommandLongSwitch("build-arg")]
    public string? BuildArg { get; set; }

    [CommandLongSwitch("cache-from")]
    public string? CacheFrom { get; set; }

    [CommandLongSwitch("cgroup-parent")]
    public string? CgroupParent { get; set; }

    [CommandLongSwitch("compress")]
    public string? Compress { get; set; }

    [CommandLongSwitch("cpu-period")]
    public string? CpuPeriod { get; set; }

    [CommandLongSwitch("cpu-quota")]
    public string? CpuQuota { get; set; }

    [CommandLongSwitch("cpu-shares")]
    public string? CpuShares { get; set; }

    [CommandLongSwitch("cpuset-cpus")]
    public string? CpusetCpus { get; set; }

    [CommandLongSwitch("cpuset-mems")]
    public string? CpusetMems { get; set; }

    [BooleanCommandSwitch("disable-content-trust")]
    public bool? DisableContentTrust { get; set; }

    [CommandLongSwitch("file")]
    public string? File { get; set; }

    [CommandLongSwitch("force-rm")]
    public string? ForceRm { get; set; }

    [CommandLongSwitch("iidfile")]
    public string? Iidfile { get; set; }

    [CommandLongSwitch("isolation")]
    public string? Isolation { get; set; }

    [CommandLongSwitch("label")]
    public string? Label { get; set; }

    [CommandLongSwitch("memory")]
    public string? Memory { get; set; }

    [CommandLongSwitch("memory-swap")]
    public string? MemorySwap { get; set; }

    [CommandLongSwitch("network")]
    public string? Network { get; set; }

    [CommandLongSwitch("no-cache")]
    public string? NoCache { get; set; }

    [CommandLongSwitch("platform")]
    public string? Platform { get; set; }

    [CommandLongSwitch("pull")]
    public string? Pull { get; set; }

    [CommandLongSwitch("quiet")]
    public string? Quiet { get; set; }

    [BooleanCommandSwitch("rm")]
    public bool? Rm { get; set; }

    [CommandLongSwitch("security-opt")]
    public string? SecurityOpt { get; set; }

    [CommandLongSwitch("shm-size")]
    public string? ShmSize { get; set; }

    [CommandLongSwitch("squash")]
    public string? Squash { get; set; }

    [CommandLongSwitch("tag")]
    public string? Tag { get; set; }

    [CommandLongSwitch("target")]
    public string? Target { get; set; }

    [CommandLongSwitch("ulimit")]
    public string? Ulimit { get; set; }

}