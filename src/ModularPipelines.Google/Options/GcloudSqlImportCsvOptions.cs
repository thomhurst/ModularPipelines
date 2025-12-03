using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sql", "import", "csv")]
public record GcloudSqlImportCsvOptions(
[property: CliArgument] string Instance,
[property: CliArgument] string Uri,
[property: CliOption("--database")] string Database,
[property: CliOption("--table")] string Table
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--columns")]
    public string[]? Columns { get; set; }

    [CliOption("--escape")]
    public string? Escape { get; set; }

    [CliOption("--fields-terminated-by")]
    public string? FieldsTerminatedBy { get; set; }

    [CliOption("--lines-terminated-by")]
    public string? LinesTerminatedBy { get; set; }

    [CliOption("--quote")]
    public string? Quote { get; set; }

    [CliOption("--user")]
    public string? User { get; set; }
}