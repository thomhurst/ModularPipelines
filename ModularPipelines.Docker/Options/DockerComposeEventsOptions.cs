using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("compose events")]
public record DockerComposeEventsOptions : DockerOptions
{
    [CommandLongSwitch("json")]
    public string? Json { get; set; }

    [CommandLongSwitch("dry-run")]
    public string? DryRun { get; set; }

}