using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("edge-cloud", "networking", "interconnects", "attachments", "dedicated", "create")]
public record GcloudEdgeCloudNetworkingInterconnectsAttachmentsDedicatedCreateOptions(
[property: CliArgument] string InterconnectAttachment,
[property: CliArgument] string Location,
[property: CliArgument] string Zone,
[property: CliOption("--interconnect")] string Interconnect
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CliOption("--mtu")]
    public string? Mtu { get; set; }

    [CliOption("--network")]
    public string? Network { get; set; }

    [CliOption("--vlan-id")]
    public string? VlanId { get; set; }
}