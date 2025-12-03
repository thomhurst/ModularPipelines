using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("datastore", "operations", "delete")]
public record GcloudDatastoreOperationsDeleteOptions(
[property: CliArgument] string Name
) : GcloudOptions;