using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ecs", "create-capacity-provider")]
public record AwsEcsCreateCapacityProviderOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--auto-scaling-group-provider")] string AutoScalingGroupProvider
) : AwsOptions
{
    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}