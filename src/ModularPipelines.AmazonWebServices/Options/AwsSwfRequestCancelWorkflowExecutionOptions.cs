using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("swf", "request-cancel-workflow-execution")]
public record AwsSwfRequestCancelWorkflowExecutionOptions(
[property: CliOption("--domain")] string Domain,
[property: CliOption("--workflow-id")] string WorkflowId
) : AwsOptions
{
    [CliOption("--run-id")]
    public string? RunId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}