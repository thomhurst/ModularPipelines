using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dns", "response-policies", "rules", "update")]
public record GcloudDnsResponsePoliciesRulesUpdateOptions(
[property: PositionalArgument] string ResponsePolicyRule,
[property: PositionalArgument] string ResponsePolicy
) : GcloudOptions
{
    [CommandSwitch("--behavior")]
    public string? Behavior { get; set; }

    [CommandSwitch("--dns-name")]
    public string? DnsName { get; set; }

    [CommandSwitch("--local-data")]
    public string[]? LocalData { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }
}