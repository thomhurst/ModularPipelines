using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
public class AzNetworkAnalytics
{
    public AzNetworkAnalytics(
        AzNetworkAnalyticsDataProduct dataProduct
    )
    {
        DataProduct = dataProduct;
    }

    public AzNetworkAnalyticsDataProduct DataProduct { get; }
}