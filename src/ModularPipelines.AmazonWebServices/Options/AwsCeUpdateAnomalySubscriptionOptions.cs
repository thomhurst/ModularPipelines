using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ce", "update-anomaly-subscription")]
public record AwsCeUpdateAnomalySubscriptionOptions(
[property: CliOption("--subscription-arn")] string SubscriptionArn
) : AwsOptions
{
    [CliOption("--threshold")]
    public double? Threshold { get; set; }

    [CliOption("--frequency")]
    public string? Frequency { get; set; }

    [CliOption("--monitor-arn-list")]
    public string[]? MonitorArnList { get; set; }

    [CliOption("--subscribers")]
    public string[]? Subscribers { get; set; }

    [CliOption("--subscription-name")]
    public string? SubscriptionName { get; set; }

    [CliOption("--threshold-expression")]
    public string? ThresholdExpression { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}