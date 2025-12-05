using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("route53domains", "associate-delegation-signer-to-domain")]
public record AwsRoute53domainsAssociateDelegationSignerToDomainOptions(
[property: CliOption("--domain-name")] string DomainName,
[property: CliOption("--signing-attributes")] string SigningAttributes
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}