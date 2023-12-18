using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("vm", "diagnostics", "get-default-config")]
public record AzVmDiagnosticsGetDefaultConfigOptions(
[property: CommandSwitch("--settings")] string Settings
) : AzOptions
{
    [BooleanCommandSwitch("--is-windows-os")]
    public bool? IsWindowsOs { get; set; }
}