using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("route53domains", "transfer-domain-to-another--account")]
public record AwsRoute53domainsTransferDomainToAnotherAwsAccountOptions(
[property: CommandSwitch("--domain-name")] string DomainName,
[property: CommandSwitch("--account-id")] string AccountId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}