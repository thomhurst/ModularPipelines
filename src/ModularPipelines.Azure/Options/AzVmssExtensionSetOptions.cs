using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("vmss", "extension", "set")]
public record AzVmssExtensionSetOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--publisher")] string Publisher,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--vmss-name")] string VmssName
) : AzOptions
{
    [CliFlag("--enable-auto-upgrade")]
    public bool? EnableAutoUpgrade { get; set; }

    [CliOption("--extension-instance-name")]
    public string? ExtensionInstanceName { get; set; }

    [CliFlag("--force-update")]
    public bool? ForceUpdate { get; set; }

    [CliFlag("--no-auto-upgrade-minor-version")]
    public bool? NoAutoUpgradeMinorVersion { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--protected-settings")]
    public string? ProtectedSettings { get; set; }

    [CliOption("--provision-after-extensions")]
    public string? ProvisionAfterExtensions { get; set; }

    [CliOption("--settings")]
    public string? Settings { get; set; }

    [CliOption("--version")]
    public string? Version { get; set; }
}