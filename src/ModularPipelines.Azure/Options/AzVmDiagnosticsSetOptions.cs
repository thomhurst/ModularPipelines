using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("vm", "diagnostics", "set")]
public record AzVmDiagnosticsSetOptions(
[property: CliOption("--settings")] string Settings
) : AzOptions
{
    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliFlag("--no-auto-upgrade-minor-version")]
    public bool? NoAutoUpgradeMinorVersion { get; set; }

    [CliOption("--protected-settings")]
    public string? ProtectedSettings { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--version")]
    public string? Version { get; set; }

    [CliOption("--vm-name")]
    public string? VmName { get; set; }
}