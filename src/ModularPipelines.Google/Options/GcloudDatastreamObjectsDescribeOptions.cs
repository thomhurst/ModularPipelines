using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datastream", "objects", "describe")]
public record GcloudDatastreamObjectsDescribeOptions(
[property: PositionalArgument] string Object,
[property: PositionalArgument] string Location,
[property: PositionalArgument] string Stream
) : GcloudOptions;