using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("support-app", "delete-slack-channel-configuration")]
public record AwsSupportAppDeleteSlackChannelConfigurationOptions(
[property: CliOption("--channel-id")] string ChannelId,
[property: CliOption("--team-id")] string TeamId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}