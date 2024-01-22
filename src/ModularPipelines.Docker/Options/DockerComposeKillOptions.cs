using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("compose", "kill")]
[ExcludeFromCodeCoverage]
public record DockerComposeKillOptions : DockerOptions
{
    [PositionalArgument(Position = Position.AfterSwitches)]
    public IEnumerable<string>? Service { get; set; }

    [BooleanCommandSwitch("--remove-orphans")]
    public bool? RemoveOrphans { get; set; }

    [CommandSwitch("--signal")]
    public string? Signal { get; set; }
}
