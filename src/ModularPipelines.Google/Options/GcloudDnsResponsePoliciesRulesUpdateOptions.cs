using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dns", "response-policies", "rules", "update")]
public record GcloudDnsResponsePoliciesRulesUpdateOptions(
[property: CliArgument] string ResponsePolicyRule,
[property: CliArgument] string ResponsePolicy
) : GcloudOptions
{
    [CliOption("--behavior")]
    public string? Behavior { get; set; }

    [CliOption("--dns-name")]
    public string? DnsName { get; set; }

    [CliOption("--local-data")]
    public string[]? LocalData { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }
}