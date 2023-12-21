using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

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