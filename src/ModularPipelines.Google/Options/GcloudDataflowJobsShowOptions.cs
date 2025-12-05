using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dataflow", "jobs", "show")]
public record GcloudDataflowJobsShowOptions(
[property: CliArgument] string JobId
) : GcloudOptions
{
    [CliFlag("--environment")]
    public bool? Environment { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }

    [CliFlag("--steps")]
    public bool? Steps { get; set; }
}