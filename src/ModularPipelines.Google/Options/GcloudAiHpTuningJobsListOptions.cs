using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ai", "hp-tuning-jobs", "list")]
public record GcloudAiHpTuningJobsListOptions : GcloudOptions
{
    [CliOption("--region")]
    public string? Region { get; set; }
}