using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;
using ModularPipelines.Azure.Options;

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