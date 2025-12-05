using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "fleet", "clusterupgrade", "update")]
public record GcloudContainerFleetClusterupgradeUpdateOptions : GcloudOptions
{
    [CliOption("--default-upgrade-soaking")]
    public string? DefaultUpgradeSoaking { get; set; }

    [CliFlag("--remove-upgrade-soaking-overrides")]
    public bool? RemoveUpgradeSoakingOverrides { get; set; }

    [CliOption("--add-upgrade-soaking-override")]
    public string? AddUpgradeSoakingOverride { get; set; }

    [CliOption("--upgrade-selector")]
    public string[]? UpgradeSelector { get; set; }

    [CliFlag("--reset-upstream-fleet")]
    public bool? ResetUpstreamFleet { get; set; }

    [CliOption("--upstream-fleet")]
    public string? UpstreamFleet { get; set; }
}