using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datastore", "indexes", "describe")]
public record GcloudDatastoreIndexesDescribeOptions(
[property: PositionalArgument] string Index
) : GcloudOptions;