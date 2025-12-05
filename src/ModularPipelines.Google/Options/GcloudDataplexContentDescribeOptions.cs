using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dataplex", "content", "describe")]
public record GcloudDataplexContentDescribeOptions(
[property: CliArgument] string Content,
[property: CliArgument] string Lake,
[property: CliArgument] string Location
) : GcloudOptions
{
    [CliOption("--view")]
    public string? View { get; set; }
}