using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("migrationhuborchestrator", "create-workflow-step-group")]
public record AwsMigrationhuborchestratorCreateWorkflowStepGroupOptions(
[property: CommandSwitch("--workflow-id")] string WorkflowId,
[property: CommandSwitch("--name")] string Name
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--next")]
    public string[]? Next { get; set; }

    [CommandSwitch("--previous")]
    public string[]? Previous { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}