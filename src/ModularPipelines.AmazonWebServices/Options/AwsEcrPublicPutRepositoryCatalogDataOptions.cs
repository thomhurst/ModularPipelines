using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ecr-public", "put-repository-catalog-data")]
public record AwsEcrPublicPutRepositoryCatalogDataOptions(
[property: CommandSwitch("--repository-name")] string RepositoryName,
[property: CommandSwitch("--catalog-data")] string CatalogData
) : AwsOptions
{
    [CommandSwitch("--registry-id")]
    public string? RegistryId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}