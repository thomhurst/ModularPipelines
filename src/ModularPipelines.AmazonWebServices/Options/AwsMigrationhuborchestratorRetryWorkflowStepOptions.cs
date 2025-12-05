using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("migrationhuborchestrator", "retry-workflow-step")]
public record AwsMigrationhuborchestratorRetryWorkflowStepOptions(
[property: CliOption("--workflow-id")] string WorkflowId,
[property: CliOption("--step-group-id")] string StepGroupId,
[property: CliOption("--id")] string Id
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}