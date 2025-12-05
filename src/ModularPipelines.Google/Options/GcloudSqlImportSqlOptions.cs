using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sql", "import", "sql")]
public record GcloudSqlImportSqlOptions(
[property: CliArgument] string Instance,
[property: CliArgument] string Uri
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--database")]
    public string? Database { get; set; }

    [CliOption("--user")]
    public string? User { get; set; }
}