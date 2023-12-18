using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("fleet", "updaterun", "create")]
public record AzFleetUpdaterunCreateOptions(
[property: CommandSwitch("--fleet-name")] string FleetName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--upgrade-type")] string UpgradeType
) : AzOptions
{
    [CommandSwitch("--kubernetes-version")]
    public string? KubernetesVersion { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--node-image-selection")]
    public string? NodeImageSelection { get; set; }

    [CommandSwitch("--stages")]
    public string? Stages { get; set; }

    [CommandSwitch("--update-strategy-name")]
    public string? UpdateStrategyName { get; set; }
}

