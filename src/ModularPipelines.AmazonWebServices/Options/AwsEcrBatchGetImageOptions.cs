using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ecr", "batch-get-image")]
public record AwsEcrBatchGetImageOptions(
[property: CliOption("--repository-name")] string RepositoryName,
[property: CliOption("--image-ids")] string[] ImageIds
) : AwsOptions
{
    [CliOption("--registry-id")]
    public string? RegistryId { get; set; }

    [CliOption("--accepted-media-types")]
    public string[]? AcceptedMediaTypes { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}