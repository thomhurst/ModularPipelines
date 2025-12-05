using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("route53domains", "renew-domain")]
public record AwsRoute53domainsRenewDomainOptions(
[property: CliOption("--domain-name")] string DomainName,
[property: CliOption("--current-expiry-year")] int CurrentExpiryYear
) : AwsOptions
{
    [CliOption("--duration-in-years")]
    public int? DurationInYears { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}