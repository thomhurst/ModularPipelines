using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("datastream", "objects", "stop-backfill")]
public record GcloudDatastreamObjectsStopBackfillOptions(
[property: CliArgument] string Object,
[property: CliArgument] string Location,
[property: CliArgument] string Stream
) : GcloudOptions;