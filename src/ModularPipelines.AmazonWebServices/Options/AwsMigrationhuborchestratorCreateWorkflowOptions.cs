using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("migrationhuborchestrator", "create-workflow")]
public record AwsMigrationhuborchestratorCreateWorkflowOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--template-id")] string TemplateId,
[property: CommandSwitch("--application-configuration-id")] string ApplicationConfigurationId,
[property: CommandSwitch("--input-parameters")] IEnumerable<KeyValue> InputParameters
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--step-targets")]
    public string[]? StepTargets { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}