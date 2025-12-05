using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("datastore", "indexes", "list")]
public record GcloudDatastoreIndexesListOptions : GcloudOptions;