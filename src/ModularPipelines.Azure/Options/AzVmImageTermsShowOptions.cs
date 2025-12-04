using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("vm", "image", "terms", "show")]
public record AzVmImageTermsShowOptions : AzOptions
{
    [CliOption("--offer")]
    public string? Offer { get; set; }

    [CliOption("--plan")]
    public string? Plan { get; set; }

    [CliOption("--publisher")]
    public string? Publisher { get; set; }

    [CliOption("--urn")]
    public string? Urn { get; set; }
}