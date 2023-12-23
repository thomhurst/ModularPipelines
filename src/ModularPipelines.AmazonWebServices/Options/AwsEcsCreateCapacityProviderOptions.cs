using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ecs", "create-capacity-provider")]
public record AwsEcsCreateCapacityProviderOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--auto-scaling-group-provider")] string AutoScalingGroupProvider
) : AwsOptions
{
    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}