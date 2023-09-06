using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("compose stop")]
[ExcludeFromCodeCoverage]
public record DockerComposeStopOptions : DockerOptions
{
    [PositionalArgument(Position = Position.AfterArguments)]
    public IEnumerable<string>? Service { get; set; }

    [CommandSwitch("--timeout")]
    public string? Timeout { get; set; }

    [BooleanCommandSwitch("--dry-run")]
    public bool? DryRun { get; set; }
}
