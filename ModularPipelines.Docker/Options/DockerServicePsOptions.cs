using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("service ps")]
public record DockerServicePsOptions : DockerOptions
{
    [CommandLongSwitch("no-resolve")]
    public string? NoResolve { get; set; }

    [CommandLongSwitch("no-trunc")]
    public string? NoTrunc { get; set; }

    [CommandLongSwitch("quiet")]
    public string? Quiet { get; set; }

}