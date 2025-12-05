using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("imagebuilder", "get-workflow-execution")]
public record AwsImagebuilderGetWorkflowExecutionOptions(
[property: CliOption("--workflow-execution-id")] string WorkflowExecutionId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}