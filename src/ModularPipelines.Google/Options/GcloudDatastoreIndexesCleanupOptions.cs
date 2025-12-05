using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("datastore", "indexes", "cleanup")]
public record GcloudDatastoreIndexesCleanupOptions(
[property: CliArgument] string IndexFile
) : GcloudOptions;