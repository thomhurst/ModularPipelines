using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("datastore", "operations", "cancel")]
public record GcloudDatastoreOperationsCancelOptions(
[property: CliArgument] string Name
) : GcloudOptions;