using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connect", "start-outbound-voice-contact")]
public record AwsConnectStartOutboundVoiceContactOptions(
[property: CommandSwitch("--destination-phone-number")] string DestinationPhoneNumber,
[property: CommandSwitch("--contact-flow-id")] string ContactFlowId,
[property: CommandSwitch("--instance-id")] string InstanceId
) : AwsOptions
{
    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--references")]
    public IEnumerable<KeyValue>? References { get; set; }

    [CommandSwitch("--related-contact-id")]
    public string? RelatedContactId { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--source-phone-number")]
    public string? SourcePhoneNumber { get; set; }

    [CommandSwitch("--queue-id")]
    public string? QueueId { get; set; }

    [CommandSwitch("--attributes")]
    public IEnumerable<KeyValue>? Attributes { get; set; }

    [CommandSwitch("--answer-machine-detection-config")]
    public string? AnswerMachineDetectionConfig { get; set; }

    [CommandSwitch("--campaign-id")]
    public string? CampaignId { get; set; }

    [CommandSwitch("--traffic-type")]
    public string? TrafficType { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}