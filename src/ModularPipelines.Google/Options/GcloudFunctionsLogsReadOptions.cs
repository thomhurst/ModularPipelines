using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("functions", "logs", "read")]
public record GcloudFunctionsLogsReadOptions(
[property: PositionalArgument] string Name
) : GcloudOptions
{
    [CommandSwitch("--end-time")]
    public string? EndTime { get; set; }

    [CommandSwitch("--execution-id")]
    public string? ExecutionId { get; set; }

    [BooleanCommandSwitch("--gen2")]
    public bool? Gen2 { get; set; }

    [CommandSwitch("--limit")]
    public string? Limit { get; set; }

    [CommandSwitch("--min-log-level")]
    public string? MinLogLevel { get; set; }

    [CommandSwitch("--region")]
    public string? Region { get; set; }

    [CommandSwitch("--start-time")]
    public string? StartTime { get; set; }
}