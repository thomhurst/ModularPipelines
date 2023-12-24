using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ce", "update-anomaly-subscription")]
public record AwsCeUpdateAnomalySubscriptionOptions(
[property: CommandSwitch("--subscription-arn")] string SubscriptionArn
) : AwsOptions
{
    [CommandSwitch("--threshold")]
    public double? Threshold { get; set; }

    [CommandSwitch("--frequency")]
    public string? Frequency { get; set; }

    [CommandSwitch("--monitor-arn-list")]
    public string[]? MonitorArnList { get; set; }

    [CommandSwitch("--subscribers")]
    public string[]? Subscribers { get; set; }

    [CommandSwitch("--subscription-name")]
    public string? SubscriptionName { get; set; }

    [CommandSwitch("--threshold-expression")]
    public string? ThresholdExpression { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}