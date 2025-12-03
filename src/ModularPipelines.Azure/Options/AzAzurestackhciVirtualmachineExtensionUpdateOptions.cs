using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("urestackhci", "virtualmachine", "extension", "update")]
public record AzAzurestackhciVirtualmachineExtensionUpdateOptions(
[property: CliOption("--name")] string Name
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

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--protected-settings")]
    public string? ProtectedSettings { get; set; }

    [CliOption("--publisher")]
    public string? Publisher { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--settings")]
    public string? Settings { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--type-handler-version")]
    public string? TypeHandlerVersion { get; set; }

    [CliOption("--virtualmachine-name")]
    public string? VirtualmachineName { get; set; }
}