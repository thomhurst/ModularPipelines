using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("migrationhuborchestrator", "create-workflow-step")]
public record AwsMigrationhuborchestratorCreateWorkflowStepOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--step-group-id")] string StepGroupId,
[property: CliOption("--workflow-id")] string WorkflowId,
[property: CliOption("--step-action-type")] string StepActionType
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--workflow-step-automation-configuration")]
    public string? WorkflowStepAutomationConfiguration { get; set; }

    [CliOption("--step-target")]
    public string[]? StepTarget { get; set; }

    [CliOption("--outputs")]
    public string[]? Outputs { get; set; }

    [CliOption("--previous")]
    public string[]? Previous { get; set; }

    [CliOption("--next")]
    public string[]? Next { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}