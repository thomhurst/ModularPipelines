using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("migrationhuborchestrator", "delete-workflow-step")]
public record AwsMigrationhuborchestratorDeleteWorkflowStepOptions(
[property: CliOption("--id")] string Id,
[property: CliOption("--step-group-id")] string StepGroupId,
[property: CliOption("--workflow-id")] string WorkflowId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}