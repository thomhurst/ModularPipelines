using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ecr", "upload-layer-part")]
public record AwsEcrUploadLayerPartOptions(
[property: CliOption("--repository-name")] string RepositoryName,
[property: CliOption("--upload-id")] string UploadId,
[property: CliOption("--part-first-byte")] long PartFirstByte,
[property: CliOption("--part-last-byte")] long PartLastByte,
[property: CliOption("--layer-part-blob")] string LayerPartBlob
) : AwsOptions
{
    [CliOption("--registry-id")]
    public string? RegistryId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}