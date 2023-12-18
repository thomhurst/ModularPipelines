using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("vm", "image", "accept-terms")]
public record AzVmImageAcceptTermsOptions(
[property: CommandSwitch("--location")] string Location,
[property: CommandSwitch("--publisher")] string Publisher
) : AzOptions
{
    [CommandSwitch("--offer")]
    public string? Offer { get; set; }

    [CommandSwitch("--plan")]
    public string? Plan { get; set; }

    [CommandSwitch("--urn")]
    public string? Urn { get; set; }
}