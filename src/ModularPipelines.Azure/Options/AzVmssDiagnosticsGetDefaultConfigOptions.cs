using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("vmss", "diagnostics", "get-default-config")]
public record AzVmssDiagnosticsGetDefaultConfigOptions(
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--settings")] string Settings,
[property: CommandSwitch("--vmss-name")] string VmssName
) : AzOptions
{
    [BooleanCommandSwitch("--is-windows-os")]
    public bool? IsWindowsOs { get; set; }
}

