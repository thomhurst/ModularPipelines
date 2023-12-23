using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("route53resolver", "import-firewall-domains")]
public record AwsRoute53resolverImportFirewallDomainsOptions(
[property: CommandSwitch("--firewall-domain-list-id")] string FirewallDomainListId,
[property: CommandSwitch("--operation")] string Operation,
[property: CommandSwitch("--domain-file-url")] string DomainFileUrl
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}