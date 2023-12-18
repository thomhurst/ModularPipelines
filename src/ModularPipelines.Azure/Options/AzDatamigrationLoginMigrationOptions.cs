using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datamigration", "login-migration")]
public record AzDatamigrationLoginMigrationOptions : AzOptions
{
    [CommandSwitch("--aad-domain-name")]
    public string? AadDomainName { get; set; }

    [CommandSwitch("--config-file-path")]
    public string? ConfigFilePath { get; set; }

    [CommandSwitch("--csv-file-path")]
    public string? CsvFilePath { get; set; }

    [CommandSwitch("--list-of-login")]
    public string? ListOfLogin { get; set; }

    [CommandSwitch("--output-folder")]
    public string? OutputFolder { get; set; }

    [CommandSwitch("--src-sql-connection-str")]
    public string? SrcSqlConnectionStr { get; set; }

    [CommandSwitch("--tgt-sql-connection-str")]
    public string? TgtSqlConnectionStr { get; set; }
}