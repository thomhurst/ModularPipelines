using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datastream", "streams", "list")]
public record GcloudDatastreamStreamsListOptions(
[property: CommandSwitch("--location")] string Location
) : GcloudOptions;