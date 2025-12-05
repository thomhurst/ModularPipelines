using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ecr", "complete-layer-upload")]
public record AwsEcrCompleteLayerUploadOptions(
[property: CliOption("--repository-name")] string RepositoryName,
[property: CliOption("--upload-id")] string UploadId,
[property: CliOption("--layer-digests")] string[] LayerDigests
) : AwsOptions
{
    [CliOption("--registry-id")]
    public string? RegistryId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}