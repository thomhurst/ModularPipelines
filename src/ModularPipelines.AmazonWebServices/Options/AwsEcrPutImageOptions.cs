using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ecr", "put-image")]
public record AwsEcrPutImageOptions(
[property: CliOption("--repository-name")] string RepositoryName,
[property: CliOption("--image-manifest")] string ImageManifest
) : AwsOptions
{
    [CliOption("--registry-id")]
    public string? RegistryId { get; set; }

    [CliOption("--image-manifest-media-type")]
    public string? ImageManifestMediaType { get; set; }

    [CliOption("--image-tag")]
    public string? ImageTag { get; set; }

    [CliOption("--image-digest")]
    public string? ImageDigest { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}