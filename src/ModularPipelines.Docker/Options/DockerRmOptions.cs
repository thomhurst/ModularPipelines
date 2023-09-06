using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("rm")]
[ExcludeFromCodeCoverage]
public record DockerRmOptions([property: PositionalArgument(Position = Position.AfterArguments)] IEnumerable<string> Containers) : DockerOptions
{
    [BooleanCommandSwitch("--force")]
    public bool? Force { get; set; }

    [CommandSwitch("--link")]
    public string? Link { get; set; }

    [CommandSwitch("--volumes")]
    public string? Volumes { get; set; }
}
