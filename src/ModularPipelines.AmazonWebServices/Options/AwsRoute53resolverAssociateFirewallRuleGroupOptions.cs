using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("route53resolver", "associate-firewall-rule-group")]
public record AwsRoute53resolverAssociateFirewallRuleGroupOptions(
[property: CommandSwitch("--firewall-rule-group-id")] string FirewallRuleGroupId,
[property: CommandSwitch("--vpc-id")] string VpcId,
[property: CommandSwitch("--priority")] int Priority,
[property: CommandSwitch("--name")] string Name
) : AwsOptions
{
    [CommandSwitch("--creator-request-id")]
    public string? CreatorRequestId { get; set; }

    [CommandSwitch("--mutation-protection")]
    public string? MutationProtection { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}