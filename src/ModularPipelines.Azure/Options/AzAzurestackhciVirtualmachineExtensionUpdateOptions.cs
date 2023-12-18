using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("urestackhci", "virtualmachine", "extension", "update")]
public record AzAzurestackhciVirtualmachineExtensionUpdateOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions
{
    [BooleanCommandSwitch("--auto-upgrade-minor")]
    public bool? AutoUpgradeMinor { get; set; }

    [BooleanCommandSwitch("--enable-auto-upgrade")]
    public bool? EnableAutoUpgrade { get; set; }

    [CommandSwitch("--extension-type")]
    public string? ExtensionType { get; set; }

    [BooleanCommandSwitch("--force-update-tag")]
    public bool? ForceUpdateTag { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--protected-settings")]
    public string? ProtectedSettings { get; set; }

    [CommandSwitch("--publisher")]
    public string? Publisher { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--settings")]
    public string? Settings { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--type-handler-version")]
    public string? TypeHandlerVersion { get; set; }

    [CommandSwitch("--virtualmachine-name")]
    public string? VirtualmachineName { get; set; }
}