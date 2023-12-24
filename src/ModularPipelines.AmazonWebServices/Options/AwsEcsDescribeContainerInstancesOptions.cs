using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ecs", "describe-container-instances")]
public record AwsEcsDescribeContainerInstancesOptions(
[property: CommandSwitch("--container-instances")] string[] ContainerInstances
) : AwsOptions
{
    [CommandSwitch("--cluster")]
    public string? Cluster { get; set; }

    [CommandSwitch("--include")]
    public string[]? Include { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}