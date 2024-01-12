using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datastore", "operations", "cancel")]
public record GcloudDatastoreOperationsCancelOptions(
[property: PositionalArgument] string Name
) : GcloudOptions;