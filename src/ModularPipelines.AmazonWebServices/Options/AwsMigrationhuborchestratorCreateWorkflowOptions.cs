using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("migrationhuborchestrator", "create-workflow")]
public record AwsMigrationhuborchestratorCreateWorkflowOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--template-id")] string TemplateId,
[property: CliOption("--application-configuration-id")] string ApplicationConfigurationId,
[property: CliOption("--input-parameters")] IEnumerable<KeyValue> InputParameters
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--step-targets")]
    public string[]? StepTargets { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}