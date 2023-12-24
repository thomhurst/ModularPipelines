using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("support-app", "delete-slack-channel-configuration")]
public record AwsSupportAppDeleteSlackChannelConfigurationOptions(
[property: CommandSwitch("--channel-id")] string ChannelId,
[property: CommandSwitch("--team-id")] string TeamId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}