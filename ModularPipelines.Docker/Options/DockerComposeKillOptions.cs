using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("compose kill")]
public record DockerComposeKillOptions : DockerOptions
{
    [CommandLongSwitch("remove-orphans")]
    public string? RemoveOrphans { get; set; }

    [CommandLongSwitch("signal")]
    public string? Signal { get; set; }

    [CommandLongSwitch("dry-run")]
    public string? DryRun { get; set; }

}