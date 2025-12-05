using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("opsworks", "deregister-ecs-cluster")]
public record AwsOpsworksDeregisterEcsClusterOptions(
[property: CliOption("--ecs-cluster-arn")] string EcsClusterArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}