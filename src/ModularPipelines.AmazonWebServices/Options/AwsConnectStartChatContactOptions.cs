using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connect", "start-chat-contact")]
public record AwsConnectStartChatContactOptions(
[property: CliOption("--instance-id")] string InstanceId,
[property: CliOption("--contact-flow-id")] string ContactFlowId,
[property: CliOption("--participant-details")] string ParticipantDetails
) : AwsOptions
{
    [CliOption("--attributes")]
    public IEnumerable<KeyValue>? Attributes { get; set; }

    [CliOption("--initial-message")]
    public string? InitialMessage { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--chat-duration-in-minutes")]
    public int? ChatDurationInMinutes { get; set; }

    [CliOption("--supported-messaging-content-types")]
    public string[]? SupportedMessagingContentTypes { get; set; }

    [CliOption("--persistent-chat")]
    public string? PersistentChat { get; set; }

    [CliOption("--related-contact-id")]
    public string? RelatedContactId { get; set; }

    [CliOption("--segment-attributes")]
    public IEnumerable<KeyValue>? SegmentAttributes { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}