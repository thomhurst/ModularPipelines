using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("datamigration", "sql-server-schema")]
public record AzDatamigrationSqlServerSchemaOptions : AzOptions
{
    [CliOption("--action")]
    public string? Action { get; set; }

    [CliOption("--config-file-path")]
    public string? ConfigFilePath { get; set; }

    [CliOption("--input-script-file-path")]
    public string? InputScriptFilePath { get; set; }

    [CliOption("--output-folder")]
    public string? OutputFolder { get; set; }

    [CliOption("--src-sql-connection-str")]
    public string? SrcSqlConnectionStr { get; set; }

    [CliOption("--tgt-sql-connection-str")]
    public string? TgtSqlConnectionStr { get; set; }
}