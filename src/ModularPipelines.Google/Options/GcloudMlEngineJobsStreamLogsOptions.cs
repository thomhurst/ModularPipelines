using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ml-engine", "jobs", "stream-logs")]
public record GcloudMlEngineJobsStreamLogsOptions(
[property: CliArgument] string Job
) : GcloudOptions
{
    [CliFlag("--allow-multiline-logs")]
    public bool? AllowMultilineLogs { get; set; }

    [CliOption("--polling-interval")]
    public string? PollingInterval { get; set; }

    [CliOption("--task-name")]
    public string? TaskName { get; set; }
}