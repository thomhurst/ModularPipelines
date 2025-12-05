using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("spanner", "databases", "execute-sql")]
public record GcloudSpannerDatabasesExecuteSqlOptions(
[property: CliArgument] string Database,
[property: CliArgument] string Instance,
[property: CliOption("--sql")] string Sql
) : GcloudOptions
{
    [CliOption("--database-role")]
    public string? DatabaseRole { get; set; }

    [CliFlag("--enable-partitioned-dml")]
    public bool? EnablePartitionedDml { get; set; }

    [CliOption("--priority")]
    public string? Priority { get; set; }

    [CliOption("--query-mode")]
    public string? QueryMode { get; set; }

    [CliOption("--timeout")]
    public string? Timeout { get; set; }

    [CliOption("--read-timestamp")]
    public string? ReadTimestamp { get; set; }

    [CliFlag("--strong")]
    public bool? Strong { get; set; }
}