using System.Diagnostics.CodeAnalysis;

namespace ModularPipelines.Azure.Services;

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