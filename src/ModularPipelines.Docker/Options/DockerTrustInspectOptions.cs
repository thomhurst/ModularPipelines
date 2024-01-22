using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("trust", "inspect")]
[ExcludeFromCodeCoverage]
public record DockerTrustInspectOptions : DockerOptions
{
    [CommandSwitch("--pretty")]
    public string? Pretty { get; set; }
}
