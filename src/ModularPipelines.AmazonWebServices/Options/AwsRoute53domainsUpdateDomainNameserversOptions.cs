using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("route53domains", "update-domain-nameservers")]
public record AwsRoute53domainsUpdateDomainNameserversOptions(
[property: CommandSwitch("--domain-name")] string DomainName,
[property: CommandSwitch("--nameservers")] string[] Nameservers
) : AwsOptions
{
    [CommandSwitch("--fi-auth-key")]
    public string? FiAuthKey { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}