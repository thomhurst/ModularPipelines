using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datastore", "operations", "delete")]
public record GcloudDatastoreOperationsDeleteOptions(
[property: PositionalArgument] string Name
) : GcloudOptions;