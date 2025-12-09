using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("wafv2", "delete-firewall-manager-rule-groups")]
public record AwsWafv2DeleteFirewallManagerRuleGroupsOptions(
[property: CliOption("--web-acl-arn")] string WebAclArn,
[property: CliOption("--web-acl-lock-token")] string WebAclLockToken
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}