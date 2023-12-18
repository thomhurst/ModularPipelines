using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

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