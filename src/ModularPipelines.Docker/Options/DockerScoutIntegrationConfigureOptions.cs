using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("scout", "integration", "configure")]
[ExcludeFromCodeCoverage]
public record DockerScoutIntegrationConfigureOptions : DockerOptions
{
    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--org")]
    public string? Org { get; set; }

    [CommandSwitch("--parameter")]
    public string? Parameter { get; set; }
}
