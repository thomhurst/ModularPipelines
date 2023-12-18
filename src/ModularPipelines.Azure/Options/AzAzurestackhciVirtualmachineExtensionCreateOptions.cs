using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("urestackhci", "virtualmachine", "extension", "create")]
public record AzAzurestackhciVirtualmachineExtensionCreateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--virtualmachine-name")] string VirtualmachineName
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

    [CommandSwitch("--inst-handler-version")]
    public string? InstHandlerVersion { get; set; }

    [CommandSwitch("--instance-view-type")]
    public string? InstanceViewType { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--protected-settings")]
    public string? ProtectedSettings { get; set; }

    [CommandSwitch("--publisher")]
    public string? Publisher { get; set; }

    [CommandSwitch("--settings")]
    public string? Settings { get; set; }

    [CommandSwitch("--status")]
    public string? Status { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--type-handler-version")]
    public string? TypeHandlerVersion { get; set; }
}