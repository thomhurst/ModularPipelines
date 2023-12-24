using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("route53resolver", "update-firewall-rule-group-association")]
public record AwsRoute53resolverUpdateFirewallRuleGroupAssociationOptions(
[property: CommandSwitch("--firewall-rule-group-association-id")] string FirewallRuleGroupAssociationId
) : AwsOptions
{
    [CommandSwitch("--priority")]
    public int? Priority { get; set; }

    [CommandSwitch("--mutation-protection")]
    public string? MutationProtection { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}