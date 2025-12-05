using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("datastore", "indexes", "describe")]
public record GcloudDatastoreIndexesDescribeOptions(
[property: CliArgument] string Index
) : GcloudOptions;