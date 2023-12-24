using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("autoscaling", "put-notification-configuration")]
public record AwsAutoscalingPutNotificationConfigurationOptions(
[property: CommandSwitch("--auto-scaling-group-name")] string AutoScalingGroupName,
[property: CommandSwitch("--topic-arn")] string TopicArn,
[property: CommandSwitch("--notification-types")] string[] NotificationTypes
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}