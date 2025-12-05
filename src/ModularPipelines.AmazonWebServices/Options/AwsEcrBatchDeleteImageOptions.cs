using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ecr", "batch-delete-image")]
public record AwsEcrBatchDeleteImageOptions(
[property: CliOption("--repository-name")] string RepositoryName,
[property: CliOption("--image-ids")] string[] ImageIds
) : AwsOptions
{
    [CliOption("--registry-id")]
    public string? RegistryId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}