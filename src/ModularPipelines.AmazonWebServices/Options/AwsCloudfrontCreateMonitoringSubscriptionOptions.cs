using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudfront", "create-monitoring-subscription")]
public record AwsCloudfrontCreateMonitoringSubscriptionOptions(
[property: CliOption("--distribution-id")] string DistributionId,
[property: CliOption("--monitoring-subscription")] string MonitoringSubscription
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}