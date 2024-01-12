using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "hub", "clusterupgrade", "create")]
public record GcloudContainerHubClusterupgradeCreateOptions : GcloudOptions
{
    [CommandSwitch("--default-upgrade-soaking")]
    public string? DefaultUpgradeSoaking { get; set; }

    [CommandSwitch("--upstream-fleet")]
    public string? UpstreamFleet { get; set; }

    [CommandSwitch("--add-upgrade-soaking-override")]
    public string? AddUpgradeSoakingOverride { get; set; }

    [CommandSwitch("--upgrade-selector")]
    public string[]? UpgradeSelector { get; set; }
}