using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sphere", "device", "certificate", "show-quota")]
public record AzSphereDeviceCertificateShowQuotaOptions : AzOptions
{
    [CommandSwitch("--device")]
    public string? Device { get; set; }
}