using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("apigee", "apis", "describe")]
public record GcloudApigeeApisDescribeOptions(
[property: CliArgument] string Api,
[property: CliArgument] string Organization
) : GcloudOptions
{
    [CliFlag("--verbose")]
    public bool? Verbose { get; set; }
}