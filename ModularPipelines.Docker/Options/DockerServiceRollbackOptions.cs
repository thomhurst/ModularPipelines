using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("service rollback")]
public record DockerServiceRollbackOptions : DockerOptions
{
    [CommandLongSwitch("detach")]
    public string? Detach { get; set; }

    [CommandLongSwitch("quiet")]
    public string? Quiet { get; set; }

}