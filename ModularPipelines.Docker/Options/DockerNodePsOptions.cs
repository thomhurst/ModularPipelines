using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("node ps")]
public record DockerNodePsOptions : DockerOptions
{
    [CommandLongSwitch("no-resolve")]
    public string? NoResolve { get; set; }

    [CommandLongSwitch("no-trunc")]
    public string? NoTrunc { get; set; }

    [CommandLongSwitch("quiet")]
    public string? Quiet { get; set; }

}