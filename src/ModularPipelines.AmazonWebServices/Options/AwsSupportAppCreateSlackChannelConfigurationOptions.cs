using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("support-app", "create-slack-channel-configuration")]
public record AwsSupportAppCreateSlackChannelConfigurationOptions(
[property: CliOption("--channel-id")] string ChannelId,
[property: CliOption("--channel-role-arn")] string ChannelRoleArn,
[property: CliOption("--notify-on-case-severity")] string NotifyOnCaseSeverity,
[property: CliOption("--team-id")] string TeamId
) : AwsOptions
{
    [CliOption("--channel-name")]
    public string? ChannelName { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}