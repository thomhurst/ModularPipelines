using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("route53resolver", "delete-firewall-domain-list")]
public record AwsRoute53resolverDeleteFirewallDomainListOptions(
[property: CliOption("--firewall-domain-list-id")] string FirewallDomainListId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}