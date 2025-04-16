using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("scout", "integration", "configure")]
[ExcludeFromCodeCoverage]
public record DockerScoutIntegrationConfigureOptions : DockerOptions
{
    [CommandSwitch("--name")]
    public virtual string? Name { get; set; }

    [CommandSwitch("--org")]
    public virtual string? Org { get; set; }

    [CommandSwitch("--parameter")]
    public virtual string? Parameter { get; set; }
}
