using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ecr-public", "batch-check-layer-availability")]
public record AwsEcrPublicBatchCheckLayerAvailabilityOptions(
[property: CliOption("--repository-name")] string RepositoryName,
[property: CliOption("--layer-digests")] string[] LayerDigests
) : AwsOptions
{
    [CliOption("--registry-id")]
    public string? RegistryId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}