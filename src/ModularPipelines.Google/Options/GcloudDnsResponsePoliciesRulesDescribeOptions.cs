using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dns", "response-policies", "rules", "describe")]
public record GcloudDnsResponsePoliciesRulesDescribeOptions(
[property: PositionalArgument] string ResponsePolicyRule,
[property: PositionalArgument] string ResponsePolicy
) : GcloudOptions
{
    [CommandSwitch("--location")]
    public string? Location { get; set; }
}