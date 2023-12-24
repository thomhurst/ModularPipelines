using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("support-app", "create-slack-channel-configuration")]
public record AwsSupportAppCreateSlackChannelConfigurationOptions(
[property: CommandSwitch("--channel-id")] string ChannelId,
[property: CommandSwitch("--channel-role-arn")] string ChannelRoleArn,
[property: CommandSwitch("--notify-on-case-severity")] string NotifyOnCaseSeverity,
[property: CommandSwitch("--team-id")] string TeamId
) : AwsOptions
{
    [CommandSwitch("--channel-name")]
    public string? ChannelName { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}