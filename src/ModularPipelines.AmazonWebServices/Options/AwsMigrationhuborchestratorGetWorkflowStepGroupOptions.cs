using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("migrationhuborchestrator", "get-workflow-step-group")]
public record AwsMigrationhuborchestratorGetWorkflowStepGroupOptions(
[property: CliOption("--id")] string Id,
[property: CliOption("--workflow-id")] string WorkflowId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}