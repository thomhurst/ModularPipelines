using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("swf", "deprecate-workflow-type")]
public record AwsSwfDeprecateWorkflowTypeOptions(
[property: CommandSwitch("--domain")] string Domain,
[property: CommandSwitch("--workflow-type")] string WorkflowType
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}