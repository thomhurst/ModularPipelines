using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("compose rm")]
[ExcludeFromCodeCoverage]
public record DockerComposeRmOptions : DockerOptions
{
    [PositionalArgument(Position = Position.AfterSwitches)]
    public IEnumerable<string>? Service { get; set; }
    [BooleanCommandSwitch("--force")]
    public bool? Force { get; set; }

    [CommandSwitch("--stop")]
    public string? Stop { get; set; }

    [CommandSwitch("--volumes")]
    public string? Volumes { get; set; }

    [BooleanCommandSwitch("--dry-run")]
    public bool? DryRun { get; set; }
}
