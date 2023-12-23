using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("migrationhuborchestrator", "delete-workflow-step")]
public record AwsMigrationhuborchestratorDeleteWorkflowStepOptions(
[property: CommandSwitch("--id")] string Id,
[property: CommandSwitch("--step-group-id")] string StepGroupId,
[property: CommandSwitch("--workflow-id")] string WorkflowId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}