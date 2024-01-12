using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "hub", "clusterupgrade", "update")]
public record GcloudContainerHubClusterupgradeUpdateOptions : GcloudOptions
{
    [CommandSwitch("--default-upgrade-soaking")]
    public string? DefaultUpgradeSoaking { get; set; }

    [BooleanCommandSwitch("--remove-upgrade-soaking-overrides")]
    public bool? RemoveUpgradeSoakingOverrides { get; set; }

    [CommandSwitch("--add-upgrade-soaking-override")]
    public string? AddUpgradeSoakingOverride { get; set; }

    [CommandSwitch("--upgrade-selector")]
    public string[]? UpgradeSelector { get; set; }

    [BooleanCommandSwitch("--reset-upstream-fleet")]
    public bool? ResetUpstreamFleet { get; set; }

    [CommandSwitch("--upstream-fleet")]
    public string? UpstreamFleet { get; set; }
}