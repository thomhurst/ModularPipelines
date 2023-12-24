using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("opsworks", "describe-ecs-clusters")]
public record AwsOpsworksDescribeEcsClustersOptions : AwsOptions
{
    [CommandSwitch("--ecs-cluster-arns")]
    public string[]? EcsClusterArns { get; set; }

    [CommandSwitch("--stack-id")]
    public string? StackId { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}