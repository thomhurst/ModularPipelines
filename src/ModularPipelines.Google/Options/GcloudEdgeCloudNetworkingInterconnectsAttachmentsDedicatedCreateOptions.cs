using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("edge-cloud", "networking", "interconnects", "attachments", "dedicated", "create")]
public record GcloudEdgeCloudNetworkingInterconnectsAttachmentsDedicatedCreateOptions(
[property: PositionalArgument] string InterconnectAttachment,
[property: PositionalArgument] string Location,
[property: PositionalArgument] string Zone,
[property: CommandSwitch("--interconnect")] string Interconnect
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CommandSwitch("--mtu")]
    public string? Mtu { get; set; }

    [CommandSwitch("--network")]
    public string? Network { get; set; }

    [CommandSwitch("--vlan-id")]
    public string? VlanId { get; set; }
}