using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("vm", "diagnostics", "set")]
public record AzVmDiagnosticsSetOptions(
[property: CommandSwitch("--settings")] string Settings
) : AzOptions
{
    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [BooleanCommandSwitch("--no-auto-upgrade-minor-version")]
    public bool? NoAutoUpgradeMinorVersion { get; set; }

    [CommandSwitch("--protected-settings")]
    public string? ProtectedSettings { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--subscription")]
    public new string? Subscription { get; set; }

    [CommandSwitch("--version")]
    public string? Version { get; set; }

    [CommandSwitch("--vm-name")]
    public string? VmName { get; set; }
}