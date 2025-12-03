using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("route53domains", "disassociate-delegation-signer-from-domain")]
public record AwsRoute53domainsDisassociateDelegationSignerFromDomainOptions(
[property: CliOption("--domain-name")] string DomainName,
[property: CliOption("--id")] string Id
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}