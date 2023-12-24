using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connect", "send-chat-integration-event")]
public record AwsConnectSendChatIntegrationEventOptions(
[property: CommandSwitch("--source-id")] string SourceId,
[property: CommandSwitch("--destination-id")] string DestinationId,
[property: CommandSwitch("--event")] string Event
) : AwsOptions
{
    [CommandSwitch("--subtype")]
    public string? Subtype { get; set; }

    [CommandSwitch("--new-session-details")]
    public string? NewSessionDetails { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}