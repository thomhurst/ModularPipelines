using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("functions", "describe")]
public record GcloudFunctionsDescribeOptions(
[property: CliArgument] string Name,
[property: CliArgument] string Region
) : GcloudOptions
{
    [CliFlag("--gen2")]
    public bool? Gen2 { get; set; }
}