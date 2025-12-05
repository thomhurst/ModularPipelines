using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "fleet", "clusterupgrade", "create")]
public record GcloudContainerFleetClusterupgradeCreateOptions : GcloudOptions
{
    [CliOption("--default-upgrade-soaking")]
    public string? DefaultUpgradeSoaking { get; set; }

    [CliOption("--upstream-fleet")]
    public string? UpstreamFleet { get; set; }

    [CliOption("--add-upgrade-soaking-override")]
    public string? AddUpgradeSoakingOverride { get; set; }

    [CliOption("--upgrade-selector")]
    public string[]? UpgradeSelector { get; set; }
}