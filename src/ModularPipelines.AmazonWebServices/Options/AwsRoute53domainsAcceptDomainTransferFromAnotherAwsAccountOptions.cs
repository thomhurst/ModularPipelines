using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("route53domains", "accept-domain-transfer-from-another--account")]
public record AwsRoute53domainsAcceptDomainTransferFromAnotherAwsAccountOptions(
[property: CliOption("--domain-name")] string DomainName,
[property: CliOption("--password")] string Password
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}