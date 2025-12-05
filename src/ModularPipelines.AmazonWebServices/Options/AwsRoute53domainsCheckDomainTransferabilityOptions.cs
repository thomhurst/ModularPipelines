using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("route53domains", "check-domain-transferability")]
public record AwsRoute53domainsCheckDomainTransferabilityOptions(
[property: CliOption("--domain-name")] string DomainName
) : AwsOptions
{
    [CliOption("--auth-code")]
    public string? AuthCode { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}