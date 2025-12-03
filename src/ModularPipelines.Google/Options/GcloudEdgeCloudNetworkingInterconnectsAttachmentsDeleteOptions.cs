using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("edge-cloud", "networking", "interconnects", "attachments", "delete")]
public record GcloudEdgeCloudNetworkingInterconnectsAttachmentsDeleteOptions(
[property: CliArgument] string InterconnectAttachment,
[property: CliArgument] string Location,
[property: CliArgument] string Zone
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }
}