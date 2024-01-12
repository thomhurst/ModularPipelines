using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("edge-cloud", "networking", "interconnects", "attachments", "describe")]
public record GcloudEdgeCloudNetworkingInterconnectsAttachmentsDescribeOptions(
[property: PositionalArgument] string InterconnectAttachment,
[property: PositionalArgument] string Location,
[property: PositionalArgument] string Zone
) : GcloudOptions;