using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("firestore", "databases", "list")]
public record GcloudFirestoreDatabasesListOptions : GcloudOptions;