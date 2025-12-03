using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("urestackhci", "virtualmachine", "extension", "create")]
public record AzAzurestackhciVirtualmachineExtensionCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--virtualmachine-name")] string VirtualmachineName
) : AzOptions
{
    [CliFlag("--auto-upgrade-minor")]
    public bool? AutoUpgradeMinor { get; set; }

    [CliFlag("--enable-auto-upgrade")]
    public bool? EnableAutoUpgrade { get; set; }

    [CliOption("--extension-type")]
    public string? ExtensionType { get; set; }

    [CliFlag("--force-update-tag")]
    public bool? ForceUpdateTag { get; set; }

    [CliOption("--inst-handler-version")]
    public string? InstHandlerVersion { get; set; }

    [CliOption("--instance-view-type")]
    public string? InstanceViewType { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--protected-settings")]
    public string? ProtectedSettings { get; set; }

    [CliOption("--publisher")]
    public string? Publisher { get; set; }

    [CliOption("--settings")]
    public string? Settings { get; set; }

    [CliOption("--status")]
    public string? Status { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--type-handler-version")]
    public string? TypeHandlerVersion { get; set; }
}