using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ai-platform", "jobs", "stream-logs")]
public record GcloudAiPlatformJobsStreamLogsOptions(
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