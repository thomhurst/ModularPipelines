using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sphere", "device", "certificate", "list")]
public record AzSphereDeviceCertificateListOptions : AzOptions
{
    [CommandSwitch("--device")]
    public string? Device { get; set; }
}