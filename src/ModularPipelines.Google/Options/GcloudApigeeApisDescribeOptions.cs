using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("apigee", "apis", "describe")]
public record GcloudApigeeApisDescribeOptions(
[property: PositionalArgument] string Api,
[property: PositionalArgument] string Organization
) : GcloudOptions
{
    [BooleanCommandSwitch("--verbose")]
    public bool? Verbose { get; set; }
}