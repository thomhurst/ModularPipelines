using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("functions", "logs", "read")]
public record GcloudFunctionsLogsReadOptions(
[property: CliArgument] string Name
) : GcloudOptions
{
    [CliOption("--end-time")]
    public string? EndTime { get; set; }

    [CliOption("--execution-id")]
    public string? ExecutionId { get; set; }

    [CliFlag("--gen2")]
    public bool? Gen2 { get; set; }

    [CliOption("--limit")]
    public string? Limit { get; set; }

    [CliOption("--min-log-level")]
    public string? MinLogLevel { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }

    [CliOption("--start-time")]
    public string? StartTime { get; set; }
}