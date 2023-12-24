using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("entityresolution", "get-matching-workflow")]
public record AwsEntityresolutionGetMatchingWorkflowOptions(
[property: CommandSwitch("--workflow-name")] string WorkflowName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}