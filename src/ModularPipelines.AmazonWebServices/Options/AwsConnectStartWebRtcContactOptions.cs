using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connect", "start-web-rtc-contact")]
public record AwsConnectStartWebRtcContactOptions(
[property: CommandSwitch("--contact-flow-id")] string ContactFlowId,
[property: CommandSwitch("--instance-id")] string InstanceId,
[property: CommandSwitch("--participant-details")] string ParticipantDetails
) : AwsOptions
{
    [CommandSwitch("--attributes")]
    public IEnumerable<KeyValue>? Attributes { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--allowed-capabilities")]
    public string? AllowedCapabilities { get; set; }

    [CommandSwitch("--related-contact-id")]
    public string? RelatedContactId { get; set; }

    [CommandSwitch("--references")]
    public IEnumerable<KeyValue>? References { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}