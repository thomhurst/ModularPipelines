using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("app", "logs", "read")]
public record GcloudAppLogsReadOptions : GcloudOptions
{
    [CliOption("--level")]
    public string? Level { get; set; }

    [CliOption("--limit")]
    public string? Limit { get; set; }

    [CliOption("--logs")]
    public string[]? Logs { get; set; }

    [CliOption("--service")]
    public string? Service { get; set; }

    [CliOption("--version")]
    public new string? Version { get; set; }
}