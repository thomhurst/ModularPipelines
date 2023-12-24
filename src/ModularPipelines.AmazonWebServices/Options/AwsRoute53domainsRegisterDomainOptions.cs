using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("route53domains", "register-domain")]
public record AwsRoute53domainsRegisterDomainOptions(
[property: CommandSwitch("--domain-name")] string DomainName,
[property: CommandSwitch("--duration-in-years")] int DurationInYears,
[property: CommandSwitch("--admin-contact")] string AdminContact,
[property: CommandSwitch("--registrant-contact")] string RegistrantContact,
[property: CommandSwitch("--tech-contact")] string TechContact
) : AwsOptions
{
    [CommandSwitch("--idn-lang-code")]
    public string? IdnLangCode { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}