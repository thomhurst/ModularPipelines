using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datastore", "operations", "describe")]
public record GcloudDatastoreOperationsDescribeOptions(
[property: PositionalArgument] string Name
) : GcloudOptions;