using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dataflow", "jobs", "list")]
public record GcloudDataflowJobsListOptions : GcloudOptions
{
    [CliOption("--created-after")]
    public string? CreatedAfter { get; set; }

    [CliOption("--created-before")]
    public string? CreatedBefore { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }

    [CliOption("--status")]
    public string? Status { get; set; }
}