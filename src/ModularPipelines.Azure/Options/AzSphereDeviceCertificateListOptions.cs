using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sphere", "device", "certificate", "list")]
public record AzSphereDeviceCertificateListOptions(
[property: CommandSwitch("--certificate")] string Certificate
) : AzOptions
{
    [CommandSwitch("--device")]
    public string? Device { get; set; }
}

