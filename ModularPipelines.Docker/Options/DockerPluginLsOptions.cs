using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("plugin ls")]
public record DockerPluginLsOptions : DockerOptions
{
    [CommandLongSwitch("no-trunc")]
    public string? NoTrunc { get; set; }

    [CommandLongSwitch("quiet")]
    public string? Quiet { get; set; }

}