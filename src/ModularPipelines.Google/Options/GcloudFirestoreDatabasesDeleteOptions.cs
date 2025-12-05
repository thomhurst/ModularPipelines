using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("firestore", "databases", "delete")]
public record GcloudFirestoreDatabasesDeleteOptions(
[property: CliOption("--database")] string Database
) : GcloudOptions
{
    [CliOption("--etag")]
    public string? Etag { get; set; }
}