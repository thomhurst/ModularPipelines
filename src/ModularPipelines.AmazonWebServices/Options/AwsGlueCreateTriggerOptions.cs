using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("glue", "create-trigger")]
public record AwsGlueCreateTriggerOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--type")] string Type,
[property: CliOption("--actions")] string[] Actions
) : AwsOptions
{
    [CliOption("--workflow-name")]
    public string? WorkflowName { get; set; }

    [CliOption("--schedule")]
    public string? Schedule { get; set; }

    [CliOption("--predicate")]
    public string? Predicate { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--event-batching-condition")]
    public string? EventBatchingCondition { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}