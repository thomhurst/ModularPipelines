using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ai", "hp-tuning-jobs", "stream-logs")]
public record GcloudAiHpTuningJobsStreamLogsOptions(
[property: PositionalArgument] string HptuningJob,
[property: PositionalArgument] string Region
) : GcloudOptions
{
    [BooleanCommandSwitch("--allow-multiline-logs")]
    public bool? AllowMultilineLogs { get; set; }

    [CommandSwitch("--polling-interval")]
    public string? PollingInterval { get; set; }

    [CommandSwitch("--task-name")]
    public string? TaskName { get; set; }
}