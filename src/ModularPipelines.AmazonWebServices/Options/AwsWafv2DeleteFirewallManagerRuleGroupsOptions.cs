using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("wafv2", "delete-firewall-manager-rule-groups")]
public record AwsWafv2DeleteFirewallManagerRuleGroupsOptions(
[property: CommandSwitch("--web-acl-arn")] string WebAclArn,
[property: CommandSwitch("--web-acl-lock-token")] string WebAclLockToken
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}