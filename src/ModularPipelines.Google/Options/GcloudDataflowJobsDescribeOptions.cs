using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dataflow", "jobs", "describe")]
public record GcloudDataflowJobsDescribeOptions(
[property: CliArgument] string JobId
) : GcloudOptions
{
    [CliFlag("--full")]
    public bool? Full { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }
}