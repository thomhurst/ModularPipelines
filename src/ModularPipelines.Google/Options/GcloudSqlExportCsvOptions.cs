using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sql", "export", "csv")]
public record GcloudSqlExportCsvOptions(
[property: PositionalArgument] string Instance,
[property: PositionalArgument] string Uri,
[property: CommandSwitch("--query")] string Query
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--database")]
    public string[]? Database { get; set; }

    [CommandSwitch("--escape")]
    public string? Escape { get; set; }

    [CommandSwitch("--fields-terminated-by")]
    public string? FieldsTerminatedBy { get; set; }

    [CommandSwitch("--lines-terminated-by")]
    public string? LinesTerminatedBy { get; set; }

    [BooleanCommandSwitch("--offload")]
    public bool? Offload { get; set; }

    [CommandSwitch("--quote")]
    public string? Quote { get; set; }
}