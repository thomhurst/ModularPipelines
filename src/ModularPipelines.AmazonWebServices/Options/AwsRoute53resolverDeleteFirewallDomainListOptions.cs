using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("route53resolver", "delete-firewall-domain-list")]
public record AwsRoute53resolverDeleteFirewallDomainListOptions(
[property: CommandSwitch("--firewall-domain-list-id")] string FirewallDomainListId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}