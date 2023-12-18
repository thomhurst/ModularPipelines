using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("vm", "extension", "set")]
public record AzVmExtensionSetOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--publisher")] string Publisher
) : AzOptions
{
    [BooleanCommandSwitch("--enable-auto-upgrade")]
    public bool? EnableAutoUpgrade { get; set; }

    [CommandSwitch("--extension-instance-name")]
    public string? ExtensionInstanceName { get; set; }

    [BooleanCommandSwitch("--force-update")]
    public bool? ForceUpdate { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [BooleanCommandSwitch("--no-auto-upgrade-minor-version")]
    public bool? NoAutoUpgradeMinorVersion { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--protected-settings")]
    public string? ProtectedSettings { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--settings")]
    public string? Settings { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }

    [CommandSwitch("--version")]
    public string? Version { get; set; }

    [CommandSwitch("--vm-name")]
    public string? VmName { get; set; }
}