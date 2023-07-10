using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("compose down")]
public record DockerComposeDownOptions : DockerOptions
{
    [CommandLongSwitch("remove-orphans")]
    public string? RemoveOrphans { get; set; }

    [CommandLongSwitch("rmi")]
    public string? Rmi { get; set; }

    [CommandLongSwitch("timeout")]
    public string? Timeout { get; set; }

    [CommandLongSwitch("volumes")]
    public string? Volumes { get; set; }

    [CommandLongSwitch("dry-run")]
    public string? DryRun { get; set; }

}