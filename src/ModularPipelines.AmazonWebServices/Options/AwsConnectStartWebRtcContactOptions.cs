using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connect", "start-web-rtc-contact")]
public record AwsConnectStartWebRtcContactOptions(
[property: CliOption("--contact-flow-id")] string ContactFlowId,
[property: CliOption("--instance-id")] string InstanceId,
[property: CliOption("--participant-details")] string ParticipantDetails
) : AwsOptions
{
    [CliOption("--attributes")]
    public IEnumerable<KeyValue>? Attributes { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--allowed-capabilities")]
    public string? AllowedCapabilities { get; set; }

    [CliOption("--related-contact-id")]
    public string? RelatedContactId { get; set; }

    [CliOption("--references")]
    public IEnumerable<KeyValue>? References { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}