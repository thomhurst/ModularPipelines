using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("buildx", "inspect")]
[ExcludeFromCodeCoverage]
public record DockerBuildxInspectOptions : DockerOptions
{
    [CommandSwitch("--bootstrap")]
    public string? Bootstrap { get; set; }
}
