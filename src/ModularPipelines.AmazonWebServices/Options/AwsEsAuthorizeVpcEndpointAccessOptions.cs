using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("es", "authorize-vpc-endpoint-access")]
public record AwsEsAuthorizeVpcEndpointAccessOptions(
[property: CliOption("--domain-name")] string DomainName,
[property: CliOption("--account")] string Account
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}