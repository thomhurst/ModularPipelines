using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dns", "response-policies", "rules", "delete")]
public record GcloudDnsResponsePoliciesRulesDeleteOptions(
[property: CliArgument] string ResponsePolicyRule,
[property: CliArgument] string ResponsePolicy
) : GcloudOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }
}