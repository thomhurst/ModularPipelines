using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("dla", "job", "submit")]
public record AzDlaJobSubmitOptions(
[property: CliOption("--job-name")] string JobName,
[property: CliOption("--script")] string Script
) : AzOptions
{
    [CliOption("--account")]
    public int? Account { get; set; }

    [CliOption("--compile-mode")]
    public string? CompileMode { get; set; }

    [CliFlag("--compile-only")]
    public bool? CompileOnly { get; set; }

    [CliOption("--degree-of-parallelism")]
    public string? DegreeOfParallelism { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--pipeline-id")]
    public string? PipelineId { get; set; }

    [CliOption("--pipeline-name")]
    public string? PipelineName { get; set; }

    [CliOption("--pipeline-uri")]
    public string? PipelineUri { get; set; }

    [CliOption("--priority")]
    public string? Priority { get; set; }

    [CliOption("--recurrence-id")]
    public string? RecurrenceId { get; set; }

    [CliOption("--recurrence-name")]
    public string? RecurrenceName { get; set; }

    [CliOption("--run-id")]
    public string? RunId { get; set; }

    [CliOption("--runtime-version")]
    public string? RuntimeVersion { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}