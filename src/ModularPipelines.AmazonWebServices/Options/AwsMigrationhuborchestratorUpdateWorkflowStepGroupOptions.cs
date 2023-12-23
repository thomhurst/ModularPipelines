using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("migrationhuborchestrator", "update-workflow-step-group")]
public record AwsMigrationhuborchestratorUpdateWorkflowStepGroupOptions(
[property: CommandSwitch("--workflow-id")] string WorkflowId,
[property: CommandSwitch("--id")] string Id
) : AwsOptions
{
    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--next")]
    public string[]? Next { get; set; }

    [CommandSwitch("--previous")]
    public string[]? Previous { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}