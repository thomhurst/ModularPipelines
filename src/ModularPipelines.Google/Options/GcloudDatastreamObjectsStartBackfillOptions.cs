using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("datastream", "objects", "start-backfill")]
public record GcloudDatastreamObjectsStartBackfillOptions(
[property: CliArgument] string Object,
[property: CliArgument] string Location,
[property: CliArgument] string Stream
) : GcloudOptions;