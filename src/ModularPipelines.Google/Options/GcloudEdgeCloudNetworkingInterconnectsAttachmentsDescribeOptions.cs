using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("edge-cloud", "networking", "interconnects", "attachments", "describe")]
public record GcloudEdgeCloudNetworkingInterconnectsAttachmentsDescribeOptions(
[property: CliArgument] string InterconnectAttachment,
[property: CliArgument] string Location,
[property: CliArgument] string Zone
) : GcloudOptions;