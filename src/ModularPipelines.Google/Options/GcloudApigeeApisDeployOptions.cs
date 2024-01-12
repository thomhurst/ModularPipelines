using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("apigee", "apis", "deploy")]
public record GcloudApigeeApisDeployOptions(
[property: PositionalArgument] string Revision,
[property: PositionalArgument] string Api,
[property: PositionalArgument] string Environment,
[property: PositionalArgument] string Organization
) : GcloudOptions
{
    [BooleanCommandSwitch("--override")]
    public bool? Override { get; set; }
}