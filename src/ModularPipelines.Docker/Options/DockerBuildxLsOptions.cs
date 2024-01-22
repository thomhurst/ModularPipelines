using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("buildx", "ls")]
[ExcludeFromCodeCoverage]
public record DockerBuildxLsOptions : DockerOptions
{
    [CommandSwitch("--builder")]
    public string? Builder { get; set; }

    [CommandSwitch("--format")]
    public string? Format { get; set; }
}
