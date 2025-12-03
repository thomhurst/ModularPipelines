using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("vm", "extension", "set")]
public record AzVmExtensionSetOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--publisher")] string Publisher
) : AzOptions
{
    [CliFlag("--enable-auto-upgrade")]
    public bool? EnableAutoUpgrade { get; set; }

    [CliOption("--extension-instance-name")]
    public string? ExtensionInstanceName { get; set; }

    [CliFlag("--force-update")]
    public bool? ForceUpdate { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliFlag("--no-auto-upgrade-minor-version")]
    public bool? NoAutoUpgradeMinorVersion { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--protected-settings")]
    public string? ProtectedSettings { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--settings")]
    public string? Settings { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--version")]
    public string? Version { get; set; }

    [CliOption("--vm-name")]
    public string? VmName { get; set; }
}