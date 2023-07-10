using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("network ls")]
public record DockerNetworkLsOptions : DockerOptions
{
    [CommandLongSwitch("no-trunc")]
    public string? NoTrunc { get; set; }

    [CommandLongSwitch("quiet")]
    public string? Quiet { get; set; }

}