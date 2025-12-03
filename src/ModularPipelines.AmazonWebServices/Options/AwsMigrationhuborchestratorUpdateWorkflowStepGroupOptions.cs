using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("migrationhuborchestrator", "update-workflow-step-group")]
public record AwsMigrationhuborchestratorUpdateWorkflowStepGroupOptions(
[property: CliOption("--workflow-id")] string WorkflowId,
[property: CliOption("--id")] string Id
) : AwsOptions
{
    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--next")]
    public string[]? Next { get; set; }

    [CliOption("--previous")]
    public string[]? Previous { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}