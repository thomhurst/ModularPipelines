using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("migrationhuborchestrator", "update-workflow-step")]
public record AwsMigrationhuborchestratorUpdateWorkflowStepOptions(
[property: CliOption("--id")] string Id,
[property: CliOption("--step-group-id")] string StepGroupId,
[property: CliOption("--workflow-id")] string WorkflowId
) : AwsOptions
{
    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--step-action-type")]
    public string? StepActionType { get; set; }

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

    [CliOption("--status")]
    public string? Status { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}