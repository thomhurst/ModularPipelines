using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloudfront", "create-monitoring-subscription")]
public record AwsCloudfrontCreateMonitoringSubscriptionOptions(
[property: CommandSwitch("--distribution-id")] string DistributionId,
[property: CommandSwitch("--monitoring-subscription")] string MonitoringSubscription
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}