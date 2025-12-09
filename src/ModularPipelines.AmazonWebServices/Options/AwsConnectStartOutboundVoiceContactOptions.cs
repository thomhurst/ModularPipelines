using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connect", "start-outbound-voice-contact")]
public record AwsConnectStartOutboundVoiceContactOptions(
[property: CliOption("--destination-phone-number")] string DestinationPhoneNumber,
[property: CliOption("--contact-flow-id")] string ContactFlowId,
[property: CliOption("--instance-id")] string InstanceId
) : AwsOptions
{
    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--references")]
    public IEnumerable<KeyValue>? References { get; set; }

    [CliOption("--related-contact-id")]
    public string? RelatedContactId { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--source-phone-number")]
    public string? SourcePhoneNumber { get; set; }

    [CliOption("--queue-id")]
    public string? QueueId { get; set; }

    [CliOption("--attributes")]
    public IEnumerable<KeyValue>? Attributes { get; set; }

    [CliOption("--answer-machine-detection-config")]
    public string? AnswerMachineDetectionConfig { get; set; }

    [CliOption("--campaign-id")]
    public string? CampaignId { get; set; }

    [CliOption("--traffic-type")]
    public string? TrafficType { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}