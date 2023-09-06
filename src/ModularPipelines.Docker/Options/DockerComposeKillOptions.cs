using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("compose kill")]
[ExcludeFromCodeCoverage]
public record DockerComposeKillOptions : DockerOptions
{
    [PositionalArgument(Position = Position.AfterArguments)]
    public IEnumerable<string>? Service { get; set; }
    [BooleanCommandSwitch("--remove-orphans")]
    public bool? RemoveOrphans { get; set; }

    [CommandSwitch("--signal")]
    public string? Signal { get; set; }

    [BooleanCommandSwitch("--dry-run")]
    public bool? DryRun { get; set; }
}
