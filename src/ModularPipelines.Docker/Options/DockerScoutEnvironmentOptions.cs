using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("scout", "environment")]
[ExcludeFromCodeCoverage]
public record DockerScoutEnvironmentOptions : DockerOptions
{
    [CommandSwitch("--org")]
    public string? Org { get; set; }

    [CommandSwitch("--output")]
    public string? Output { get; set; }

    [CommandSwitch("--platform")]
    public string? Platform { get; set; }
}
