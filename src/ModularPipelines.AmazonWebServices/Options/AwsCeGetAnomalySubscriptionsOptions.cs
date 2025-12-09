using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ce", "get-anomaly-subscriptions")]
public record AwsCeGetAnomalySubscriptionsOptions : AwsOptions
{
    [CliOption("--subscription-arn-list")]
    public string[]? SubscriptionArnList { get; set; }

    [CliOption("--monitor-arn")]
    public string? MonitorArn { get; set; }

    [CliOption("--next-page-token")]
    public string? NextPageToken { get; set; }

    [CliOption("--max-results")]
    public int? MaxResults { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}