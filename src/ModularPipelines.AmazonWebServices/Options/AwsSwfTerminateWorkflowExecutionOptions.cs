using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("swf", "terminate-workflow-execution")]
public record AwsSwfTerminateWorkflowExecutionOptions(
[property: CliOption("--domain")] string Domain,
[property: CliOption("--workflow-id")] string WorkflowId
) : AwsOptions
{
    [CliOption("--run-id")]
    public string? RunId { get; set; }

    [CliOption("--reason")]
    public string? Reason { get; set; }

    [CliOption("--details")]
    public string? Details { get; set; }

    [CliOption("--child-policy")]
    public string? ChildPolicy { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}