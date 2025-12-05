using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ecs", "update-capacity-provider")]
public record AwsEcsUpdateCapacityProviderOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--auto-scaling-group-provider")] string AutoScalingGroupProvider
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}