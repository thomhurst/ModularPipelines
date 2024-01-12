using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "security-policies", "rules", "remove-preconfig-waf-exclusion")]
public record GcloudComputeSecurityPoliciesRulesRemovePreconfigWafExclusionOptions(
[property: PositionalArgument] string Priority,
[property: CommandSwitch("--target-rule-set")] string TargetRuleSet
) : GcloudOptions
{
    [CommandSwitch("--region")]
    public string? Region { get; set; }

    [CommandSwitch("--request-cookie-to-exclude")]
    public string[]? RequestCookieToExclude { get; set; }

    [CommandSwitch("--request-header-to-exclude")]
    public string[]? RequestHeaderToExclude { get; set; }

    [CommandSwitch("--request-query-param-to-exclude")]
    public string[]? RequestQueryParamToExclude { get; set; }

    [CommandSwitch("--request-uri-to-exclude")]
    public string[]? RequestUriToExclude { get; set; }

    [CommandSwitch("--security-policy")]
    public string? SecurityPolicy { get; set; }

    [CommandSwitch("--target-rule-ids")]
    public string[]? TargetRuleIds { get; set; }
}