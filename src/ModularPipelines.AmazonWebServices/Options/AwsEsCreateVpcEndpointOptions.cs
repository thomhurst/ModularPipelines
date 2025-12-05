using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("es", "create-vpc-endpoint")]
public record AwsEsCreateVpcEndpointOptions(
[property: CliOption("--domain-arn")] string DomainArn,
[property: CliOption("--vpc-options")] string VpcOptions
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}