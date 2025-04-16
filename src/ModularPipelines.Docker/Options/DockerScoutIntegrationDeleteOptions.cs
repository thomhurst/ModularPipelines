using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("scout", "integration", "delete")]
[ExcludeFromCodeCoverage]
public record DockerScoutIntegrationDeleteOptions : DockerOptions
{
    [CommandSwitch("--name")]
    public virtual string? Name { get; set; }

    [CommandSwitch("--org")]
    public virtual string? Org { get; set; }
}
