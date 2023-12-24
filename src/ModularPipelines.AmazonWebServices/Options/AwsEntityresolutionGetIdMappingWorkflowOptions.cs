using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("entityresolution", "get-id-mapping-workflow")]
public record AwsEntityresolutionGetIdMappingWorkflowOptions(
[property: CommandSwitch("--workflow-name")] string WorkflowName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}