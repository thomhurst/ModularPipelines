using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("route53domains", "accept-domain-transfer-from-another--account")]
public record AwsRoute53domainsAcceptDomainTransferFromAnotherAwsAccountOptions(
[property: CommandSwitch("--domain-name")] string DomainName,
[property: CommandSwitch("--password")] string Password
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}