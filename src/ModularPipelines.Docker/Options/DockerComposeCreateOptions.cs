using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("compose", "create")]
[ExcludeFromCodeCoverage]
public record DockerComposeCreateOptions : DockerOptions
{
    [PositionalArgument(Position = Position.AfterSwitches)]
    public IEnumerable<string>? Service { get; set; }

    [BooleanCommandSwitch("--build")]
    public bool? Build { get; set; }

    [BooleanCommandSwitch("--force-recreate")]
    public bool? ForceRecreate { get; set; }

    [BooleanCommandSwitch("--no-build")]
    public bool? NoBuild { get; set; }

    [BooleanCommandSwitch("--no-recreate")]
    public bool? NoRecreate { get; set; }

    [BooleanCommandSwitch("--pull")]
    public bool? Pull { get; set; }

    [BooleanCommandSwitch("--remove-orphans")]
    public bool? RemoveOrphans { get; set; }

    [CommandSwitch("--scale")]
    public string? Scale { get; set; }
}
