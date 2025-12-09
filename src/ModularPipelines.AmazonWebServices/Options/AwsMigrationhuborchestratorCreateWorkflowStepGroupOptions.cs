using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("migrationhuborchestrator", "create-workflow-step-group")]
public record AwsMigrationhuborchestratorCreateWorkflowStepGroupOptions(
[property: CliOption("--workflow-id")] string WorkflowId,
[property: CliOption("--name")] string Name
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--next")]
    public string[]? Next { get; set; }

    [CliOption("--previous")]
    public string[]? Previous { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}