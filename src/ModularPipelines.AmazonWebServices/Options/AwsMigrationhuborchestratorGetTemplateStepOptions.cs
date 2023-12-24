using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("migrationhuborchestrator", "get-template-step")]
public record AwsMigrationhuborchestratorGetTemplateStepOptions(
[property: CommandSwitch("--id")] string Id,
[property: CommandSwitch("--template-id")] string TemplateId,
[property: CommandSwitch("--step-group-id")] string StepGroupId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}