using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("alexaforbusiness", "send-announcement")]
public record AwsAlexaforbusinessSendAnnouncementOptions(
[property: CommandSwitch("--room-filters")] string[] RoomFilters,
[property: CommandSwitch("--content")] string Content
) : AwsOptions
{
    [CommandSwitch("--time-to-live-in-seconds")]
    public int? TimeToLiveInSeconds { get; set; }

    [CommandSwitch("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}