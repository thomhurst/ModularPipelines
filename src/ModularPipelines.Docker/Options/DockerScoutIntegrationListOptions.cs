using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("scout", "integration", "list")]
[ExcludeFromCodeCoverage]
public record DockerScoutIntegrationListOptions : DockerOptions
{
    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--org")]
    public string? Org { get; set; }
}
