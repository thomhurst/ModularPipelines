using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("entityresolution", "delete-id-mapping-workflow")]
public record AwsEntityresolutionDeleteIdMappingWorkflowOptions(
[property: CliOption("--workflow-name")] string WorkflowName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}