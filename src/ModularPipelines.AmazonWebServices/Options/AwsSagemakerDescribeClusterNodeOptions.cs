using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker", "describe-cluster-node")]
public record AwsSagemakerDescribeClusterNodeOptions(
[property: CommandSwitch("--cluster-name")] string ClusterName,
[property: CommandSwitch("--node-id")] string NodeId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}