using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("partnercenter")]
public class AzPartnercenterMarketplace
{
    public AzPartnercenterMarketplace(
        AzPartnercenterMarketplaceOffer offer
    )
    {
        Offer = offer;
    }

    public AzPartnercenterMarketplaceOffer Offer { get; }
}