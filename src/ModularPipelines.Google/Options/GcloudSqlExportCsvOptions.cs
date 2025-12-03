using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sql", "export", "csv")]
public record GcloudSqlExportCsvOptions(
[property: CliArgument] string Instance,
[property: CliArgument] string Uri,
[property: CliOption("--query")] string Query
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--database")]
    public string[]? Database { get; set; }

    [CliOption("--escape")]
    public string? Escape { get; set; }

    [CliOption("--fields-terminated-by")]
    public string? FieldsTerminatedBy { get; set; }

    [CliOption("--lines-terminated-by")]
    public string? LinesTerminatedBy { get; set; }

    [CliFlag("--offload")]
    public bool? Offload { get; set; }

    [CliOption("--quote")]
    public string? Quote { get; set; }
}