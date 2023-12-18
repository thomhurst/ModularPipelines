using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("vm", "diagnostics", "get-default-config")]
public record AzVmDiagnosticsGetDefaultConfigOptions : AzOptions
{
    [BooleanCommandSwitch("--is-windows-os")]
    public bool? IsWindowsOs { get; set; }
}