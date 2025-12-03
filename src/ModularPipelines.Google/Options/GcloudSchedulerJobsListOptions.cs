using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("scheduler", "jobs", "list")]
public record GcloudSchedulerJobsListOptions : GcloudOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }
}