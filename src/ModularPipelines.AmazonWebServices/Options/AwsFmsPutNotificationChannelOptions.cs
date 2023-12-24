using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("fms", "put-notification-channel")]
public record AwsFmsPutNotificationChannelOptions(
[property: CommandSwitch("--sns-topic-arn")] string SnsTopicArn,
[property: CommandSwitch("--sns-role-name")] string SnsRoleName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}