using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ai", "indexes", "upsert-datapoints")]
public record GcloudAiIndexesUpsertDatapointsOptions(
[property: CliArgument] string Index,
[property: CliArgument] string Region,
[property: CliOption("--datapoints-from-file")] string DatapointsFromFile
) : GcloudOptions;