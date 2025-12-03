using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("run", "jobs", "executions", "list")]
public record GcloudRunJobsExecutionsListOptions : GcloudOptions
{
    [CliOption("--job")]
    public string? Job { get; set; }

    [CliOption("--namespace")]
    public string? Namespace { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }
}