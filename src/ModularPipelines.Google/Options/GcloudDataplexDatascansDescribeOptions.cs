using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dataplex", "datascans", "describe")]
public record GcloudDataplexDatascansDescribeOptions(
[property: CliArgument] string Datascan,
[property: CliArgument] string Location
) : GcloudOptions
{
    [CliOption("--view")]
    public string? View { get; set; }
}