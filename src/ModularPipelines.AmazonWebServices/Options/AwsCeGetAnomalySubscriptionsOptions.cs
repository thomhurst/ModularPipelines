using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ce", "get-anomaly-subscriptions")]
public record AwsCeGetAnomalySubscriptionsOptions : AwsOptions
{
    [CommandSwitch("--subscription-arn-list")]
    public string[]? SubscriptionArnList { get; set; }

    [CommandSwitch("--monitor-arn")]
    public string? MonitorArn { get; set; }

    [CommandSwitch("--next-page-token")]
    public string? NextPageToken { get; set; }

    [CommandSwitch("--max-results")]
    public int? MaxResults { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}