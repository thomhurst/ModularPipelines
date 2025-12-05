using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("migrationhuborchestrator", "get-template-step")]
public record AwsMigrationhuborchestratorGetTemplateStepOptions(
[property: CliOption("--id")] string Id,
[property: CliOption("--template-id")] string TemplateId,
[property: CliOption("--step-group-id")] string StepGroupId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}