using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("vmss", "diagnostics", "get-default-config")]
public record AzVmssDiagnosticsGetDefaultConfigOptions : AzOptions
{
    [BooleanCommandSwitch("--is-windows-os")]
    public bool? IsWindowsOs { get; set; }
}