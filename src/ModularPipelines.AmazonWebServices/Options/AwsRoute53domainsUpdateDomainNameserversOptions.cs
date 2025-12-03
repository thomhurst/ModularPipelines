using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("route53domains", "update-domain-nameservers")]
public record AwsRoute53domainsUpdateDomainNameserversOptions(
[property: CliOption("--domain-name")] string DomainName,
[property: CliOption("--nameservers")] string[] Nameservers
) : AwsOptions
{
    [CliOption("--fi-auth-key")]
    public string? FiAuthKey { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}