using ModularPipelines.Attributes;
using ModularPipelines.FileSystem;
using ModularPipelines.Options;

namespace ModularPipelines.Docker.Options;

public record DockerBuildOptions(string DockerfileFolder) : CommandLineToolOptions("docker")
{
    [CommandSwitch("t")]
    public string? Tag { get; init; }

    [CommandSwitch("c")]
    public string? CpuShares { get; init; }

    [CommandSwitch("m")]
    public string? MemoryLimit { get; init; }

    [BooleanCommandSwitch("isolation")]
    public bool Isolation { get; init; }

    [BooleanCommandSwitch("quiet")]
    public bool Quiet { get; init; }

    [BooleanCommandSwitch("pull")]
    public bool Pull { get; init; }

    [BooleanCommandSwitch("no-cache")]
    public bool NoCache { get; init; }

    [BooleanCommandSwitch("squash")]
    public bool Squash { get; init; }

    [CommandLongSwitch("network")]
    public string? Network { get; init; }

    [CommandLongSwitch("target")]
    public string? Target { get; init; }

    [CommandLongSwitch("output")]
    public string? Output { get; init; }

    [CommandLongSwitch("platform")]
    public string? Platform { get; init; }

    [CommandLongSwitch("shm-size")]
    public string? ShmSize { get; init; }

    [CommandLongSwitch("build-arg", SwitchValueSeparator = " ")]
    public IEnumerable<string>? BuildArguments { get; init; }

    [CommandLongSwitch("add-host")]
    public IEnumerable<string>? AddHosts { get; init; }

    [CommandSwitch("f")]
    public string? Dockerfile { get; init; }
}
