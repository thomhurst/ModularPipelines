using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("es", "authorize-vpc-endpoint-access")]
public record AwsEsAuthorizeVpcEndpointAccessOptions(
[property: CommandSwitch("--domain-name")] string DomainName,
[property: CommandSwitch("--account")] string Account
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}