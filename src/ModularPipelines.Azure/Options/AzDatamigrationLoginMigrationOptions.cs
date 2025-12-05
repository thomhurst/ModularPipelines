using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("datamigration", "login-migration")]
public record AzDatamigrationLoginMigrationOptions : AzOptions
{
    [CliOption("--aad-domain-name")]
    public string? AadDomainName { get; set; }

    [CliOption("--config-file-path")]
    public string? ConfigFilePath { get; set; }

    [CliOption("--csv-file-path")]
    public string? CsvFilePath { get; set; }

    [CliOption("--list-of-login")]
    public string? ListOfLogin { get; set; }

    [CliOption("--output-folder")]
    public string? OutputFolder { get; set; }

    [CliOption("--src-sql-connection-str")]
    public string? SrcSqlConnectionStr { get; set; }

    [CliOption("--tgt-sql-connection-str")]
    public string? TgtSqlConnectionStr { get; set; }
}