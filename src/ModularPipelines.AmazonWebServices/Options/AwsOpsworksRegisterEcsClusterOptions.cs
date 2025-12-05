using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("opsworks", "register-ecs-cluster")]
public record AwsOpsworksRegisterEcsClusterOptions(
[property: CliOption("--ecs-cluster-arn")] string EcsClusterArn,
[property: CliOption("--stack-id")] string StackId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}