using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("fleet", "updaterun", "create")]
public record AzFleetUpdaterunCreateOptions(
[property: CliOption("--fleet-name")] string FleetName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--upgrade-type")] string UpgradeType
) : AzOptions
{
    [CliOption("--kubernetes-version")]
    public string? KubernetesVersion { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--node-image-selection")]
    public string? NodeImageSelection { get; set; }

    [CliOption("--stages")]
    public string? Stages { get; set; }

    [CliOption("--update-strategy-name")]
    public string? UpdateStrategyName { get; set; }
}