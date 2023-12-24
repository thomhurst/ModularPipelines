using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("glue", "create-trigger")]
public record AwsGlueCreateTriggerOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--type")] string Type,
[property: CommandSwitch("--actions")] string[] Actions
) : AwsOptions
{
    [CommandSwitch("--workflow-name")]
    public string? WorkflowName { get; set; }

    [CommandSwitch("--schedule")]
    public string? Schedule { get; set; }

    [CommandSwitch("--predicate")]
    public string? Predicate { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--event-batching-condition")]
    public string? EventBatchingCondition { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}