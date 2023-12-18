using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("vmss", "extension", "list")]
public record AzVmssExtensionListOptions(
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--vmss-name")] string VmssName
) : AzOptions
{
    [BooleanCommandSwitch("--enable-auto-upgrade")]
    public bool? EnableAutoUpgrade { get; set; }

    [CommandSwitch("--extension-instance-name")]
    public string? ExtensionInstanceName { get; set; }

    [BooleanCommandSwitch("--force-update")]
    public bool? ForceUpdate { get; set; }

    [BooleanCommandSwitch("--no-auto-upgrade-minor-version")]
    public bool? NoAutoUpgradeMinorVersion { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--protected-settings")]
    public string? ProtectedSettings { get; set; }

    [CommandSwitch("--provision-after-extensions")]
    public string? ProvisionAfterExtensions { get; set; }

    [CommandSwitch("--settings")]
    public string? Settings { get; set; }

    [CommandSwitch("--version")]
    public string? Version { get; set; }
}