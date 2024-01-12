using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datastream", "objects", "list")]
public record GcloudDatastreamObjectsListOptions(
[property: CommandSwitch("--stream")] string Stream,
[property: CommandSwitch("--location")] string Location
) : GcloudOptions;