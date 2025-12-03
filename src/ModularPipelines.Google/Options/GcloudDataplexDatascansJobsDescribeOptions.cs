using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dataplex", "datascans", "jobs", "describe")]
public record GcloudDataplexDatascansJobsDescribeOptions(
[property: CliArgument] string Job,
[property: CliArgument] string Datascan,
[property: CliArgument] string Location
) : GcloudOptions
{
    [CliOption("--view")]
    public string? View { get; set; }
}