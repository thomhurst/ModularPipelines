using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("route53domains", "associate-delegation-signer-to-domain")]
public record AwsRoute53domainsAssociateDelegationSignerToDomainOptions(
[property: CommandSwitch("--domain-name")] string DomainName,
[property: CommandSwitch("--signing-attributes")] string SigningAttributes
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}