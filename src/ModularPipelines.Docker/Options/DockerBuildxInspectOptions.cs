using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("buildx inspect")]
[ExcludeFromCodeCoverage]
public record DockerBuildxInspectOptions : DockerOptions
{
    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? Name { get; set; }

    [CommandSwitch("--bootstrap")]
    public string? Bootstrap { get; set; }

    [CommandSwitch("--builder")]
    public string? Builder { get; set; }
}