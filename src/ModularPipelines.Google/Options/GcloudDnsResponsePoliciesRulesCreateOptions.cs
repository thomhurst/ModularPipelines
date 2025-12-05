using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dns", "response-policies", "rules", "create")]
public record GcloudDnsResponsePoliciesRulesCreateOptions(
[property: CliArgument] string ResponsePolicyRule,
[property: CliArgument] string ResponsePolicy,
[property: CliOption("--dns-name")] string DnsName
) : GcloudOptions
{
    [CliOption("--behavior")]
    public string? Behavior { get; set; }

    [CliOption("--local-data")]
    public string[]? LocalData { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }
}