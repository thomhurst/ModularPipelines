using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("route53resolver", "import-firewall-domains")]
public record AwsRoute53resolverImportFirewallDomainsOptions(
[property: CliOption("--firewall-domain-list-id")] string FirewallDomainListId,
[property: CliOption("--operation")] string Operation,
[property: CliOption("--domain-file-url")] string DomainFileUrl
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}