using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ecr", "batch-check-layer-availability")]
public record AwsEcrBatchCheckLayerAvailabilityOptions(
[property: CommandSwitch("--repository-name")] string RepositoryName,
[property: CommandSwitch("--layer-digests")] string[] LayerDigests
) : AwsOptions
{
    [CommandSwitch("--registry-id")]
    public string? RegistryId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}