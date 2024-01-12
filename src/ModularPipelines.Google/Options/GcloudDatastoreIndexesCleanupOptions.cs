using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datastore", "indexes", "cleanup")]
public record GcloudDatastoreIndexesCleanupOptions(
[property: PositionalArgument] string IndexFile
) : GcloudOptions;