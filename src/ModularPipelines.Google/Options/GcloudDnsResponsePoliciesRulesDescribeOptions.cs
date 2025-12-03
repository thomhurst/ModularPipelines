using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dns", "response-policies", "rules", "describe")]
public record GcloudDnsResponsePoliciesRulesDescribeOptions(
[property: CliArgument] string ResponsePolicyRule,
[property: CliArgument] string ResponsePolicy
) : GcloudOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }
}