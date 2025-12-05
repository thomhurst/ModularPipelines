using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("support-app", "update-slack-channel-configuration")]
public record AwsSupportAppUpdateSlackChannelConfigurationOptions(
[property: CliOption("--channel-id")] string ChannelId,
[property: CliOption("--team-id")] string TeamId
) : AwsOptions
{
    [CliOption("--channel-name")]
    public string? ChannelName { get; set; }

    [CliOption("--channel-role-arn")]
    public string? ChannelRoleArn { get; set; }

    [CliOption("--notify-on-case-severity")]
    public string? NotifyOnCaseSeverity { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}