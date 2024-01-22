using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("scout", "integration", "list")]
[ExcludeFromCodeCoverage]
public record DockerScoutIntegrationListOptions : DockerOptions
{
    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? Integration { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--org")]
    public string? Org { get; set; }
}
