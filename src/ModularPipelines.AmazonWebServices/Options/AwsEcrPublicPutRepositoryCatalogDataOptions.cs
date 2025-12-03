using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ecr-public", "put-repository-catalog-data")]
public record AwsEcrPublicPutRepositoryCatalogDataOptions(
[property: CliOption("--repository-name")] string RepositoryName,
[property: CliOption("--catalog-data")] string CatalogData
) : AwsOptions
{
    [CliOption("--registry-id")]
    public string? RegistryId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}