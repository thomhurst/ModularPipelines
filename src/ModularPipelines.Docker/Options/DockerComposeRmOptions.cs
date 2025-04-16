using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("compose", "rm")]
[ExcludeFromCodeCoverage]
public record DockerComposeRmOptions : DockerOptions
{
    [PositionalArgument(Position = Position.AfterSwitches)]
    public IEnumerable<string>? Service { get; set; }

    [BooleanCommandSwitch("--all")]
    public virtual bool? All { get; set; }

    [BooleanCommandSwitch("--force")]
    public virtual bool? Force { get; set; }

    [CommandSwitch("--stop")]
    public virtual string? Stop { get; set; }

    [CommandSwitch("--volumes")]
    public virtual string? Volumes { get; set; }
}
