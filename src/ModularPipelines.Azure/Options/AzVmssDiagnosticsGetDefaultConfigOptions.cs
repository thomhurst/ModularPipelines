using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("vmss", "diagnostics", "get-default-config")]
public record AzVmssDiagnosticsGetDefaultConfigOptions : AzOptions
{
    [CliFlag("--is-windows-os")]
    public bool? IsWindowsOs { get; set; }
}