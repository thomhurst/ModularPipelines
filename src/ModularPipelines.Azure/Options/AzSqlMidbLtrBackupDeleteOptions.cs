using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("sql", "midb", "ltr-backup", "delete")]
public record AzSqlMidbLtrBackupDeleteOptions : AzOptions
{
    [CliOption("--backup-id")]
    public string? BackupId { get; set; }

    [CliOption("--database")]
    public string? Database { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--managed-instance")]
    public string? ManagedInstance { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}