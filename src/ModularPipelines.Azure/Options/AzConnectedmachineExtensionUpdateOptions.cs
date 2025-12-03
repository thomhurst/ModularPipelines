using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connectedmachine", "extension", "update")]
public record AzConnectedmachineExtensionUpdateOptions : AzOptions
{
    [CliOption("--add")]
    public string? Add { get; set; }

    [CliFlag("--auto-upgrade")]
    public bool? AutoUpgrade { get; set; }

    [CliFlag("--auto-upgrade-minor")]
    public bool? AutoUpgradeMinor { get; set; }

    [CliOption("--extension-name")]
    public string? ExtensionName { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliFlag("--force-update-tag")]
    public bool? ForceUpdateTag { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--instance-view")]
    public string? InstanceView { get; set; }

    [CliOption("--machine-name")]
    public string? MachineName { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--protected-settings")]
    public string? ProtectedSettings { get; set; }

    [CliOption("--publisher")]
    public string? Publisher { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliOption("--settings")]
    public string? Settings { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--type")]
    public string? Type { get; set; }

    [CliOption("--type-handler-version")]
    public string? TypeHandlerVersion { get; set; }
}