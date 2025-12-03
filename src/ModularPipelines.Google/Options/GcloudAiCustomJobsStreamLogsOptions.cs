using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ai", "custom-jobs", "stream-logs")]
public record GcloudAiCustomJobsStreamLogsOptions(
[property: CliArgument] string CustomJob,
[property: CliArgument] string Region
) : GcloudOptions
{
    [CliFlag("--allow-multiline-logs")]
    public bool? AllowMultilineLogs { get; set; }

    [CliOption("--polling-interval")]
    public string? PollingInterval { get; set; }

    [CliOption("--task-name")]
    public string? TaskName { get; set; }
}