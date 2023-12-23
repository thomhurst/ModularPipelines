using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker", "describe-edge-packaging-job")]
public record AwsSagemakerDescribeEdgePackagingJobOptions(
[property: CommandSwitch("--edge-packaging-job-name")] string EdgePackagingJobName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}