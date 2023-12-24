using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("route53domains", "update-domain-contact")]
public record AwsRoute53domainsUpdateDomainContactOptions(
[property: CommandSwitch("--domain-name")] string DomainName
) : AwsOptions
{
    [CommandSwitch("--admin-contact")]
    public string? AdminContact { get; set; }

    [CommandSwitch("--registrant-contact")]
    public string? RegistrantContact { get; set; }

    [CommandSwitch("--tech-contact")]
    public string? TechContact { get; set; }

    [CommandSwitch("--consent")]
    public string? Consent { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}