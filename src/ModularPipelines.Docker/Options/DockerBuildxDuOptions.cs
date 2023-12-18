using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("buildx du")]
[ExcludeFromCodeCoverage]
public record DockerBuildxDuOptions : DockerOptions
{
    [CommandSwitch("--filter")]
    public string? Filter { get; set; }

    [CommandSwitch("--verbose")]
    public string? Verbose { get; set; }

    [CommandSwitch("--builder")]
    public string? Builder { get; set; }
}