using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("bigtable", "backups", "create")]
public record GcloudBigtableBackupsCreateOptions(
[property: CliArgument] string Backup,
[property: CliArgument] string Cluster,
[property: CliArgument] string Instance,
[property: CliOption("--table")] string Table,
[property: CliOption("--expiration-date")] string ExpirationDate,
[property: CliOption("--retention-period")] string RetentionPeriod
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }
}