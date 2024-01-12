using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dataplex", "datascans", "jobs", "describe")]
public record GcloudDataplexDatascansJobsDescribeOptions(
[property: PositionalArgument] string Job,
[property: PositionalArgument] string Datascan,
[property: PositionalArgument] string Location
) : GcloudOptions
{
    [CommandSwitch("--view")]
    public string? View { get; set; }
}