using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("datastream", "objects", "describe")]
public record GcloudDatastreamObjectsDescribeOptions(
[property: CliArgument] string Object,
[property: CliArgument] string Location,
[property: CliArgument] string Stream
) : GcloudOptions;