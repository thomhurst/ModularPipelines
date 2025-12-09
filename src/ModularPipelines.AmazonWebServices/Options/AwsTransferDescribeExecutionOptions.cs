using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("transfer", "describe-execution")]
public record AwsTransferDescribeExecutionOptions(
[property: CliOption("--execution-id")] string ExecutionId,
[property: CliOption("--workflow-id")] string WorkflowId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}