using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ai", "indexes", "remove-datapoints")]
public record GcloudAiIndexesRemoveDatapointsOptions(
[property: PositionalArgument] string Index,
[property: PositionalArgument] string Region,
[property: CommandSwitch("--datapoint-ids")] string[] DatapointIds,
[property: CommandSwitch("--datapoints-from-file")] string DatapointsFromFile
) : GcloudOptions;