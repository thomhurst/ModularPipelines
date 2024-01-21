using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("buildx", "version")]
[ExcludeFromCodeCoverage]
public record DockerBuildxVersionOptions : DockerOptions
{
    [CommandSwitch("--builder")]
    public string? Builder { get; set; }
}
