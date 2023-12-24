using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("migrationhuborchestrator", "create-workflow-step")]
public record AwsMigrationhuborchestratorCreateWorkflowStepOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--step-group-id")] string StepGroupId,
[property: CommandSwitch("--workflow-id")] string WorkflowId,
[property: CommandSwitch("--step-action-type")] string StepActionType
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--workflow-step-automation-configuration")]
    public string? WorkflowStepAutomationConfiguration { get; set; }

    [CommandSwitch("--step-target")]
    public string[]? StepTarget { get; set; }

    [CommandSwitch("--outputs")]
    public string[]? Outputs { get; set; }

    [CommandSwitch("--previous")]
    public string[]? Previous { get; set; }

    [CommandSwitch("--next")]
    public string[]? Next { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}