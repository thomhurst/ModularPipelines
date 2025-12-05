using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("wafv2", "get-rate-based-statement-managed-keys")]
public record AwsWafv2GetRateBasedStatementManagedKeysOptions(
[property: CliOption("--scope")] string Scope,
[property: CliOption("--web-acl-name")] string WebAclName,
[property: CliOption("--web-acl-id")] string WebAclId,
[property: CliOption("--rule-name")] string RuleName
) : AwsOptions
{
    [CliOption("--rule-group-rule-name")]
    public string? RuleGroupRuleName { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}