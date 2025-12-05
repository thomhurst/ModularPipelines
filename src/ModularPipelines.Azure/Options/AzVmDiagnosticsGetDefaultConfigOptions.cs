using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("vm", "diagnostics", "get-default-config")]
public record AzVmDiagnosticsGetDefaultConfigOptions : AzOptions
{
    [CliFlag("--is-windows-os")]
    public bool? IsWindowsOs { get; set; }
}