using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ecs", "update-capacity-provider")]
public record AwsEcsUpdateCapacityProviderOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--auto-scaling-group-provider")] string AutoScalingGroupProvider
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}