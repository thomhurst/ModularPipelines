using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("edge-cloud", "networking", "interconnects", "attachments", "list")]
public record GcloudEdgeCloudNetworkingInterconnectsAttachmentsListOptions(
[property: CommandSwitch("--zone")] string Zone,
[property: CommandSwitch("--location")] string Location
) : GcloudOptions;