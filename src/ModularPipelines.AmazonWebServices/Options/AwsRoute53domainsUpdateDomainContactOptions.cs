using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("route53domains", "update-domain-contact")]
public record AwsRoute53domainsUpdateDomainContactOptions(
[property: CliOption("--domain-name")] string DomainName
) : AwsOptions
{
    [CliOption("--admin-contact")]
    public string? AdminContact { get; set; }

    [CliOption("--registrant-contact")]
    public string? RegistrantContact { get; set; }

    [CliOption("--tech-contact")]
    public string? TechContact { get; set; }

    [CliOption("--consent")]
    public string? Consent { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}