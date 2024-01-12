using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ai-platform", "jobs", "stream-logs")]
public record GcloudAiPlatformJobsStreamLogsOptions(
[property: PositionalArgument] string Job
) : GcloudOptions
{
    [BooleanCommandSwitch("--allow-multiline-logs")]
    public bool? AllowMultilineLogs { get; set; }

    [CommandSwitch("--polling-interval")]
    public string? PollingInterval { get; set; }

    [CommandSwitch("--task-name")]
    public string? TaskName { get; set; }
}