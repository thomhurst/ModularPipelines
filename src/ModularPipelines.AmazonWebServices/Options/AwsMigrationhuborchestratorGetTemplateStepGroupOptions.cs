using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("migrationhuborchestrator", "get-template-step-group")]
public record AwsMigrationhuborchestratorGetTemplateStepGroupOptions(
[property: CommandSwitch("--template-id")] string TemplateId,
[property: CommandSwitch("--id")] string Id
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}