using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("support-app", "update-slack-channel-configuration")]
public record AwsSupportAppUpdateSlackChannelConfigurationOptions(
[property: CommandSwitch("--channel-id")] string ChannelId,
[property: CommandSwitch("--team-id")] string TeamId
) : AwsOptions
{
    [CommandSwitch("--channel-name")]
    public string? ChannelName { get; set; }

    [CommandSwitch("--channel-role-arn")]
    public string? ChannelRoleArn { get; set; }

    [CommandSwitch("--notify-on-case-severity")]
    public string? NotifyOnCaseSeverity { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}