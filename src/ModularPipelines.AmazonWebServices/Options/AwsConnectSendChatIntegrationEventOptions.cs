using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connect", "send-chat-integration-event")]
public record AwsConnectSendChatIntegrationEventOptions(
[property: CliOption("--source-id")] string SourceId,
[property: CliOption("--destination-id")] string DestinationId,
[property: CliOption("--event")] string Event
) : AwsOptions
{
    [CliOption("--subtype")]
    public string? Subtype { get; set; }

    [CliOption("--new-session-details")]
    public string? NewSessionDetails { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}