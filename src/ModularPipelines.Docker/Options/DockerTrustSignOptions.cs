using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("trust", "sign")]
[ExcludeFromCodeCoverage]
public record DockerTrustSignOptions : DockerOptions
{
    [CommandSwitch("--local")]
    public string? Local { get; set; }
}
