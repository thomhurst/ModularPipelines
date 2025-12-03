using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("run", "jobs", "list")]
public record GcloudRunJobsListOptions : GcloudOptions
{
    [CliOption("--namespace")]
    public string? Namespace { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }
}