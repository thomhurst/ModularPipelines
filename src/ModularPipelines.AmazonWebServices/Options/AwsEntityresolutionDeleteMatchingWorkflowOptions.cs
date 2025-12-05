using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("entityresolution", "delete-matching-workflow")]
public record AwsEntityresolutionDeleteMatchingWorkflowOptions(
[property: CliOption("--workflow-name")] string WorkflowName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}