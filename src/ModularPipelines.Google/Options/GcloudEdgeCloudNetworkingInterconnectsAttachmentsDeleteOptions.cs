using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("edge-cloud", "networking", "interconnects", "attachments", "delete")]
public record GcloudEdgeCloudNetworkingInterconnectsAttachmentsDeleteOptions(
[property: PositionalArgument] string InterconnectAttachment,
[property: PositionalArgument] string Location,
[property: PositionalArgument] string Zone
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }
}