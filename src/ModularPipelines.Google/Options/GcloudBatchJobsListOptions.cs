using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("batch", "jobs", "list")]
public record GcloudBatchJobsListOptions : GcloudOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }
}