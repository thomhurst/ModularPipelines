using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("opsworks", "describe-ecs-clusters")]
public record AwsOpsworksDescribeEcsClustersOptions : AwsOptions
{
    [CliOption("--ecs-cluster-arns")]
    public string[]? EcsClusterArns { get; set; }

    [CliOption("--stack-id")]
    public string? StackId { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}