using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connectedmachine", "extension", "create")]
public record AzConnectedmachineExtensionCreateOptions(
[property: CliOption("--extension-name")] string ExtensionName,
[property: CliOption("--machine-name")] string MachineName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliFlag("--auto-upgrade")]
    public bool? AutoUpgrade { get; set; }

    [CliFlag("--auto-upgrade-minor")]
    public bool? AutoUpgradeMinor { get; set; }

    [CliFlag("--force-update-tag")]
    public bool? ForceUpdateTag { get; set; }

    [CliOption("--instance-view")]
    public string? InstanceView { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--protected-settings")]
    public string? ProtectedSettings { get; set; }

    [CliOption("--publisher")]
    public string? Publisher { get; set; }

    [CliOption("--settings")]
    public string? Settings { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--type")]
    public string? Type { get; set; }

    [CliOption("--type-handler-version")]
    public string? TypeHandlerVersion { get; set; }
}