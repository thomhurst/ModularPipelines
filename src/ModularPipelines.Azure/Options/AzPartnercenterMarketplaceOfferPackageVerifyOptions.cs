using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("partnercenter", "marketplace", "offer", "package", "verify")]
public record AzPartnercenterMarketplaceOfferPackageVerifyOptions(
[property: CommandSwitch("--offer-id")] string OfferId
) : AzOptions
{
    [CommandSwitch("--manifest-file")]
    public string? ManifestFile { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }
}