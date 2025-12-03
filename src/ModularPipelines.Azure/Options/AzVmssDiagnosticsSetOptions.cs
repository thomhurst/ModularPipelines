using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("vmss", "diagnostics", "set")]
public record AzVmssDiagnosticsSetOptions(
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--settings")] string Settings,
[property: CliOption("--vmss-name")] string VmssName
) : AzOptions
{
    [CliFlag("--no-auto-upgrade-minor-version")]
    public bool? NoAutoUpgradeMinorVersion { get; set; }

    [CliOption("--protected-settings")]
    public string? ProtectedSettings { get; set; }

    [CliOption("--version")]
    public string? Version { get; set; }
}