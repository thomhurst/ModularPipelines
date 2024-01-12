using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sql", "import", "csv")]
public record GcloudSqlImportCsvOptions(
[property: PositionalArgument] string Instance,
[property: PositionalArgument] string Uri,
[property: CommandSwitch("--database")] string Database,
[property: CommandSwitch("--table")] string Table
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--columns")]
    public string[]? Columns { get; set; }

    [CommandSwitch("--escape")]
    public string? Escape { get; set; }

    [CommandSwitch("--fields-terminated-by")]
    public string? FieldsTerminatedBy { get; set; }

    [CommandSwitch("--lines-terminated-by")]
    public string? LinesTerminatedBy { get; set; }

    [CommandSwitch("--quote")]
    public string? Quote { get; set; }

    [CommandSwitch("--user")]
    public string? User { get; set; }
}