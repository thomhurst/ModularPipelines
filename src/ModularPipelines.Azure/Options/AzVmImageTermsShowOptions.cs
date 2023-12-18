using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("vm", "image", "terms", "show")]
public record AzVmImageTermsShowOptions : AzOptions
{
    [CommandSwitch("--offer")]
    public string? Offer { get; set; }

    [CommandSwitch("--plan")]
    public string? Plan { get; set; }

    [CommandSwitch("--publisher")]
    public string? Publisher { get; set; }

    [CommandSwitch("--urn")]
    public string? Urn { get; set; }
}