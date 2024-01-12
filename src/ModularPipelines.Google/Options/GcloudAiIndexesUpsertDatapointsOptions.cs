using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ai", "indexes", "upsert-datapoints")]
public record GcloudAiIndexesUpsertDatapointsOptions(
[property: PositionalArgument] string Index,
[property: PositionalArgument] string Region,
[property: CommandSwitch("--datapoints-from-file")] string DatapointsFromFile
) : GcloudOptions;