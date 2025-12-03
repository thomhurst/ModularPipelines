using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dataflow", "jobs", "update-options")]
public record GcloudDataflowJobsUpdateOptionsOptions(
[property: CliArgument] string JobId
) : GcloudOptions
{
    [CliOption("--max-num-workers")]
    public string? MaxNumWorkers { get; set; }

    [CliOption("--min-num-workers")]
    public string? MinNumWorkers { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }
}