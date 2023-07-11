using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("compose version")]
public record DockerComposeVersionOptions : DockerOptions
{

    [CommandSwitch("--format")]
    public string? Format { get; set; }


    [CommandSwitch("--short")]
    public string? Short { get; set; }

    [BooleanCommandSwitch("--dry-run")]
    public bool? DryRun { get; set; }

}