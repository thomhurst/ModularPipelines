using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datastream", "objects", "start-backfill")]
public record GcloudDatastreamObjectsStartBackfillOptions(
[property: PositionalArgument] string Object,
[property: PositionalArgument] string Location,
[property: PositionalArgument] string Stream
) : GcloudOptions;