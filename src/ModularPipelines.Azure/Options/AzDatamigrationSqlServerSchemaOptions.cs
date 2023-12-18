using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datamigration", "sql-server-schema")]
public record AzDatamigrationSqlServerSchemaOptions : AzOptions
{
    [CommandSwitch("--action")]
    public string? Action { get; set; }

    [CommandSwitch("--config-file-path")]
    public string? ConfigFilePath { get; set; }

    [CommandSwitch("--input-script-file-path")]
    public string? InputScriptFilePath { get; set; }

    [CommandSwitch("--output-folder")]
    public string? OutputFolder { get; set; }

    [CommandSwitch("--src-sql-connection-str")]
    public string? SrcSqlConnectionStr { get; set; }

    [CommandSwitch("--tgt-sql-connection-str")]
    public string? TgtSqlConnectionStr { get; set; }
}