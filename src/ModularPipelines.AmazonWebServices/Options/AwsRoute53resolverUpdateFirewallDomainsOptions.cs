using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("route53resolver", "update-firewall-domains")]
public record AwsRoute53resolverUpdateFirewallDomainsOptions(
[property: CommandSwitch("--firewall-domain-list-id")] string FirewallDomainListId,
[property: CommandSwitch("--operation")] string Operation,
[property: CommandSwitch("--domains")] string[] Domains
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}