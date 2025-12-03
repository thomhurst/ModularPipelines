using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ecs", "update-container-instances-state")]
public record AwsEcsUpdateContainerInstancesStateOptions(
[property: CliOption("--container-instances")] string[] ContainerInstances,
[property: CliOption("--status")] string Status
) : AwsOptions
{
    [CliOption("--cluster")]
    public string? Cluster { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}