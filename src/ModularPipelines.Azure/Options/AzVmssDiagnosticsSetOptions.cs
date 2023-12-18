using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("vmss", "diagnostics", "set")]
public record AzVmssDiagnosticsSetOptions(
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--settings")] string Settings,
[property: CommandSwitch("--vmss-name")] string VmssName
) : AzOptions
{
    [BooleanCommandSwitch("--no-auto-upgrade-minor-version")]
    public bool? NoAutoUpgradeMinorVersion { get; set; }

    [CommandSwitch("--protected-settings")]
    public string? ProtectedSettings { get; set; }

    [CommandSwitch("--version")]
    public string? Version { get; set; }
}