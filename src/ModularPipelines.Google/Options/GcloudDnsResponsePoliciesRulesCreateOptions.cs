using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dns", "response-policies", "rules", "create")]
public record GcloudDnsResponsePoliciesRulesCreateOptions(
[property: PositionalArgument] string ResponsePolicyRule,
[property: PositionalArgument] string ResponsePolicy,
[property: CommandSwitch("--dns-name")] string DnsName
) : GcloudOptions
{
    [CommandSwitch("--behavior")]
    public string? Behavior { get; set; }

    [CommandSwitch("--local-data")]
    public string[]? LocalData { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }
}