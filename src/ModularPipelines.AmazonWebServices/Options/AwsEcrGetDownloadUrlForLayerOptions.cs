using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ecr", "get-download-url-for-layer")]
public record AwsEcrGetDownloadUrlForLayerOptions(
[property: CliOption("--repository-name")] string RepositoryName,
[property: CliOption("--layer-digest")] string LayerDigest
) : AwsOptions
{
    [CliOption("--registry-id")]
    public string? RegistryId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}