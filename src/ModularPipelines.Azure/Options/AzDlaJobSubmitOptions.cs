using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dla", "job", "submit")]
public record AzDlaJobSubmitOptions(
[property: CommandSwitch("--job-name")] string JobName,
[property: CommandSwitch("--script")] string Script
) : AzOptions
{
    [CommandSwitch("--account")]
    public int? Account { get; set; }

    [CommandSwitch("--compile-mode")]
    public string? CompileMode { get; set; }

    [BooleanCommandSwitch("--compile-only")]
    public bool? CompileOnly { get; set; }

    [CommandSwitch("--degree-of-parallelism")]
    public string? DegreeOfParallelism { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--pipeline-id")]
    public string? PipelineId { get; set; }

    [CommandSwitch("--pipeline-name")]
    public string? PipelineName { get; set; }

    [CommandSwitch("--pipeline-uri")]
    public string? PipelineUri { get; set; }

    [CommandSwitch("--priority")]
    public string? Priority { get; set; }

    [CommandSwitch("--recurrence-id")]
    public string? RecurrenceId { get; set; }

    [CommandSwitch("--recurrence-name")]
    public string? RecurrenceName { get; set; }

    [CommandSwitch("--run-id")]
    public string? RunId { get; set; }

    [CommandSwitch("--runtime-version")]
    public string? RuntimeVersion { get; set; }

    [CommandSwitch("--subscription")]
    public new string? Subscription { get; set; }
}