using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ecr-public", "create-repository")]
public record AwsEcrPublicCreateRepositoryOptions(
[property: CliOption("--repository-name")] string RepositoryName
) : AwsOptions
{
    [CliOption("--catalog-data")]
    public string? CatalogData { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}