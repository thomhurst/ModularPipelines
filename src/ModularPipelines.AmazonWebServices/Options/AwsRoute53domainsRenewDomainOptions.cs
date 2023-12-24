using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("route53domains", "renew-domain")]
public record AwsRoute53domainsRenewDomainOptions(
[property: CommandSwitch("--domain-name")] string DomainName,
[property: CommandSwitch("--current-expiry-year")] int CurrentExpiryYear
) : AwsOptions
{
    [CommandSwitch("--duration-in-years")]
    public int? DurationInYears { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}