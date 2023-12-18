using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sphere", "device", "certificate", "delete")]
public record AzSphereDeviceCertificateDeleteOptions(
[property: CommandSwitch("--certificate")] string Certificate
) : AzOptions
{
    [CommandSwitch("--device")]
    public string? Device { get; set; }
}