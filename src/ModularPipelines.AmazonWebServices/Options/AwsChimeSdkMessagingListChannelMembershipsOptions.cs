using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("chime-sdk-messaging", "list-channel-memberships")]
public record AwsChimeSdkMessagingListChannelMembershipsOptions(
[property: CliOption("--channel-arn")] string ChannelArn,
[property: CliOption("--chime-bearer")] string ChimeBearer
) : AwsOptions
{
    [CliOption("--type")]
    public string? Type { get; set; }

    [CliOption("--max-results")]
    public int? MaxResults { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--sub-channel-id")]
    public string? SubChannelId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}