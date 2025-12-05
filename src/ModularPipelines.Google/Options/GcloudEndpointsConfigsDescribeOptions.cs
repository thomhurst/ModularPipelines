using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("endpoints", "configs", "describe")]
public record GcloudEndpointsConfigsDescribeOptions(
[property: CliArgument] string ConfigId
) : GcloudOptions
{
    [CliOption("--service")]
    public string? Service { get; set; }
}