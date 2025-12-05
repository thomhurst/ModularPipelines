using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("marketplace-deployment", "put-deployment-parameter")]
public record AwsMarketplaceDeploymentPutDeploymentParameterOptions(
[property: CliOption("--agreement-id")] string AgreementId,
[property: CliOption("--catalog")] string Catalog,
[property: CliOption("--deployment-parameter")] string DeploymentParameter,
[property: CliOption("--product-id")] string ProductId
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--expiration-date")]
    public long? ExpirationDate { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}