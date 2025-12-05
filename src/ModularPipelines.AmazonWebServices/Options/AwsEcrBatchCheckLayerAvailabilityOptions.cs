using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ecr", "batch-check-layer-availability")]
public record AwsEcrBatchCheckLayerAvailabilityOptions(
[property: CliOption("--repository-name")] string RepositoryName,
[property: CliOption("--layer-digests")] string[] LayerDigests
) : AwsOptions
{
    [CliOption("--registry-id")]
    public string? RegistryId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}