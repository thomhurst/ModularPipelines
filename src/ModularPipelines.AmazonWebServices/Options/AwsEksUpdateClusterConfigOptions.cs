using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("eks", "update-cluster-config")]
public record AwsEksUpdateClusterConfigOptions(
[property: CliOption("--name")] string Name
) : AwsOptions
{
    [CliOption("--resources-vpc-config")]
    public string? ResourcesVpcConfig { get; set; }

    [CliOption("--logging")]
    public string? Logging { get; set; }

    [CliOption("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CliOption("--access-config")]
    public string? AccessConfig { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}