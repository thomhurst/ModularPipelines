using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("compose port")]
[ExcludeFromCodeCoverage]
public record DockerComposePortOptions([property: PositionalArgument(Position = Position.AfterSwitches)] string Service, [property: PositionalArgument(Position = Position.AfterSwitches)] string Privateport) : DockerOptions
{
    [CommandSwitch("--index")]
    public string? Index { get; set; }

    [CommandSwitch("--protocol")]
    public string? Protocol { get; set; }

    [BooleanCommandSwitch("--dry-run")]
    public bool? DryRun { get; set; }
}