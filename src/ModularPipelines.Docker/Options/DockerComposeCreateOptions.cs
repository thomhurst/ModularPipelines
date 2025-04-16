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
    public virtual bool? Build { get; set; }

    [BooleanCommandSwitch("--force-recreate")]
    public virtual bool? ForceRecreate { get; set; }

    [BooleanCommandSwitch("--no-build")]
    public virtual bool? NoBuild { get; set; }

    [BooleanCommandSwitch("--no-recreate")]
    public virtual bool? NoRecreate { get; set; }

    [BooleanCommandSwitch("--pull")]
    public virtual bool? Pull { get; set; }

    [BooleanCommandSwitch("--remove-orphans")]
    public virtual bool? RemoveOrphans { get; set; }

    [CommandSwitch("--scale")]
    public virtual string? Scale { get; set; }
}
