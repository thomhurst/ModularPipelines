using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("migrationhuborchestrator", "delete-workflow-step-group")]
public record AwsMigrationhuborchestratorDeleteWorkflowStepGroupOptions(
[property: CommandSwitch("--workflow-id")] string WorkflowId,
[property: CommandSwitch("--id")] string Id
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}