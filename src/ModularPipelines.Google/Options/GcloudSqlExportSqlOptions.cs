using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sql", "export", "sql")]
public record GcloudSqlExportSqlOptions(
[property: CliArgument] string Instance,
[property: CliArgument] string Uri
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--database")]
    public string[]? Database { get; set; }

    [CliFlag("--offload")]
    public bool? Offload { get; set; }

    [CliOption("--table")]
    public string[]? Table { get; set; }
}