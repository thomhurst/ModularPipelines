using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("partnercenter", "marketplace", "offer", "package", "verify")]
public record AzPartnercenterMarketplaceOfferPackageVerifyOptions(
[property: CliOption("--offer-id")] string OfferId
) : AzOptions
{
    [CliOption("--manifest-file")]
    public string? ManifestFile { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }
}