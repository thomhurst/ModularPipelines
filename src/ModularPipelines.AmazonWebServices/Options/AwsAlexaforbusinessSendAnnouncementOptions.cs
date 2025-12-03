using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("alexaforbusiness", "send-announcement")]
public record AwsAlexaforbusinessSendAnnouncementOptions(
[property: CliOption("--room-filters")] string[] RoomFilters,
[property: CliOption("--content")] string Content
) : AwsOptions
{
    [CliOption("--time-to-live-in-seconds")]
    public int? TimeToLiveInSeconds { get; set; }

    [CliOption("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}