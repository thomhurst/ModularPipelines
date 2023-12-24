using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("migrationhuborchestrator", "retry-workflow-step")]
public record AwsMigrationhuborchestratorRetryWorkflowStepOptions(
[property: CommandSwitch("--workflow-id")] string WorkflowId,
[property: CommandSwitch("--step-group-id")] string StepGroupId,
[property: CommandSwitch("--id")] string Id
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}