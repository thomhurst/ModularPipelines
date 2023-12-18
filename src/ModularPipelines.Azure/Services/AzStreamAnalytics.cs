using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
public class AzStreamAnalytics
{
    public AzStreamAnalytics(
        AzStreamAnalyticsCluster cluster,
        AzStreamAnalyticsFunction function,
        AzStreamAnalyticsInput input,
        AzStreamAnalyticsJob job,
        AzStreamAnalyticsOutput output,
        AzStreamAnalyticsPrivateEndpoint privateEndpoint,
        AzStreamAnalyticsSubscription subscription,
        AzStreamAnalyticsTransformation transformation
    )
    {
        Cluster = cluster;
        Function = function;
        Input = input;
        Job = job;
        Output = output;
        PrivateEndpoint = privateEndpoint;
        Subscription = subscription;
        Transformation = transformation;
    }

    public AzStreamAnalyticsCluster Cluster { get; }

    public AzStreamAnalyticsFunction Function { get; }

    public AzStreamAnalyticsInput Input { get; }

    public AzStreamAnalyticsJob Job { get; }

    public AzStreamAnalyticsOutput Output { get; }

    public AzStreamAnalyticsPrivateEndpoint PrivateEndpoint { get; }

    public AzStreamAnalyticsSubscription Subscription { get; }

    public AzStreamAnalyticsTransformation Transformation { get; }
}