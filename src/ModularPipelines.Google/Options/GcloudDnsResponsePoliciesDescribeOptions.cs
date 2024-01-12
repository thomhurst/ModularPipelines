using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dns", "response-policies", "describe")]
public record GcloudDnsResponsePoliciesDescribeOptions(
[property: PositionalArgument] string ResponsePolicies
) : GcloudOptions
{
    [CommandSwitch("--location")]
    public string? Location { get; set; }
}