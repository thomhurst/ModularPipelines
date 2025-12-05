using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("autoscaling", "delete-notification-configuration")]
public record AwsAutoscalingDeleteNotificationConfigurationOptions(
[property: CliOption("--auto-scaling-group-name")] string AutoScalingGroupName,
[property: CliOption("--topic-arn")] string TopicArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}