using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("swf", "describe-workflow-execution")]
public record AwsSwfDescribeWorkflowExecutionOptions(
[property: CliOption("--domain")] string Domain,
[property: CliOption("--execution")] string Execution
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}