using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ai", "indexes", "remove-datapoints")]
public record GcloudAiIndexesRemoveDatapointsOptions(
[property: CliArgument] string Index,
[property: CliArgument] string Region,
[property: CliOption("--datapoint-ids")] string[] DatapointIds,
[property: CliOption("--datapoints-from-file")] string DatapointsFromFile
) : GcloudOptions;