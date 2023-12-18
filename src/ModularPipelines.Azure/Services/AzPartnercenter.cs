using System.Diagnostics.CodeAnalysis;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
public class AzPartnercenter
{
    public AzPartnercenter(
        AzPartnercenterMarketplace marketplace
    )
    {
        Marketplace = marketplace;
    }

    public AzPartnercenterMarketplace Marketplace { get; }
}