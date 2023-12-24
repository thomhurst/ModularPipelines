using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("route53domains", "transfer-domain")]
public record AwsRoute53domainsTransferDomainOptions(
[property: CommandSwitch("--domain-name")] string DomainName,
[property: CommandSwitch("--duration-in-years")] int DurationInYears,
[property: CommandSwitch("--admin-contact")] string AdminContact,
[property: CommandSwitch("--registrant-contact")] string RegistrantContact,
[property: CommandSwitch("--tech-contact")] string TechContact
) : AwsOptions
{
    [CommandSwitch("--idn-lang-code")]
    public string? IdnLangCode { get; set; }

    [CommandSwitch("--nameservers")]
    public string[]? Nameservers { get; set; }

    [CommandSwitch("--auth-code")]
    public string? AuthCode { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}