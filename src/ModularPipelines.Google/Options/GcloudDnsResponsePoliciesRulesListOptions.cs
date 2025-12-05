using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dns", "response-policies", "rules", "list")]
public record GcloudDnsResponsePoliciesRulesListOptions(
[property: CliArgument] string ResponsePolicies
) : GcloudOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }
}