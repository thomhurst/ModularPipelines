using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("spanner", "databases", "sessions", "list")]
public record GcloudSpannerDatabasesSessionsListOptions(
[property: CliOption("--database")] string Database,
[property: CliOption("--instance")] string Instance
) : GcloudOptions
{
    [CliOption("--server-filter")]
    public string? ServerFilter { get; set; }
}