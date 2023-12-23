using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connect", "start-chat-contact")]
public record AwsConnectStartChatContactOptions(
[property: CommandSwitch("--instance-id")] string InstanceId,
[property: CommandSwitch("--contact-flow-id")] string ContactFlowId,
[property: CommandSwitch("--participant-details")] string ParticipantDetails
) : AwsOptions
{
    [CommandSwitch("--attributes")]
    public IEnumerable<KeyValue>? Attributes { get; set; }

    [CommandSwitch("--initial-message")]
    public string? InitialMessage { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--chat-duration-in-minutes")]
    public int? ChatDurationInMinutes { get; set; }

    [CommandSwitch("--supported-messaging-content-types")]
    public string[]? SupportedMessagingContentTypes { get; set; }

    [CommandSwitch("--persistent-chat")]
    public string? PersistentChat { get; set; }

    [CommandSwitch("--related-contact-id")]
    public string? RelatedContactId { get; set; }

    [CommandSwitch("--segment-attributes")]
    public IEnumerable<KeyValue>? SegmentAttributes { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}