using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ecs", "update-container-instances-state")]
public record AwsEcsUpdateContainerInstancesStateOptions(
[property: CommandSwitch("--container-instances")] string[] ContainerInstances,
[property: CommandSwitch("--status")] string Status
) : AwsOptions
{
    [CommandSwitch("--cluster")]
    public string? Cluster { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}