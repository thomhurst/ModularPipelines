using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "security-policies", "rules", "add-preconfig-waf-exclusion")]
public record GcloudComputeSecurityPoliciesRulesAddPreconfigWafExclusionOptions(
[property: CliArgument] string Priority,
[property: CliOption("--target-rule-set")] string TargetRuleSet
) : GcloudOptions
{
    [CliOption("--region")]
    public string? Region { get; set; }

    [CliOption("--request-cookie-to-exclude")]
    public string[]? RequestCookieToExclude { get; set; }

    [CliOption("--request-header-to-exclude")]
    public string[]? RequestHeaderToExclude { get; set; }

    [CliOption("--request-query-param-to-exclude")]
    public string[]? RequestQueryParamToExclude { get; set; }

    [CliOption("--request-uri-to-exclude")]
    public string[]? RequestUriToExclude { get; set; }

    [CliOption("--security-policy")]
    public string? SecurityPolicy { get; set; }

    [CliOption("--target-rule-ids")]
    public string[]? TargetRuleIds { get; set; }
}