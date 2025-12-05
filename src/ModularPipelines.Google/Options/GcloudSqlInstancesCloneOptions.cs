using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sql", "instances", "clone")]
public record GcloudSqlInstancesCloneOptions(
[property: CliArgument] string Source,
[property: CliArgument] string Destination
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--bin-log-file-name")]
    public string? BinLogFileName { get; set; }

    [CliOption("--bin-log-position")]
    public string? BinLogPosition { get; set; }

    [CliOption("--point-in-time")]
    public string? PointInTime { get; set; }

    [CliOption("--preferred-zone")]
    public string? PreferredZone { get; set; }

    [CliOption("--restore-database-name")]
    public string? RestoreDatabaseName { get; set; }
}