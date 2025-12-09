using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("route53domains", "register-domain")]
public record AwsRoute53domainsRegisterDomainOptions(
[property: CliOption("--domain-name")] string DomainName,
[property: CliOption("--duration-in-years")] int DurationInYears,
[property: CliOption("--admin-contact")] string AdminContact,
[property: CliOption("--registrant-contact")] string RegistrantContact,
[property: CliOption("--tech-contact")] string TechContact
) : AwsOptions
{
    [CliOption("--idn-lang-code")]
    public string? IdnLangCode { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}