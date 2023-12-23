using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("migrationhuborchestrator", "update-workflow-step")]
public record AwsMigrationhuborchestratorUpdateWorkflowStepOptions(
[property: CommandSwitch("--id")] string Id,
[property: CommandSwitch("--step-group-id")] string StepGroupId,
[property: CommandSwitch("--workflow-id")] string WorkflowId
) : AwsOptions
{
    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--step-action-type")]
    public string? StepActionType { get; set; }

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

    [CommandSwitch("--status")]
    public string? Status { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}