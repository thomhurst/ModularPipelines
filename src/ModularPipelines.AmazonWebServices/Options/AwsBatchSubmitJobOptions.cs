using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("batch", "submit-job")]
public record AwsBatchSubmitJobOptions(
[property: CliOption("--job-name")] string JobName,
[property: CliOption("--job-queue")] string JobQueue,
[property: CliOption("--job-definition")] string JobDefinition
) : AwsOptions
{
    [CliOption("--share-identifier")]
    public string? ShareIdentifier { get; set; }

    [CliOption("--scheduling-priority-override")]
    public int? SchedulingPriorityOverride { get; set; }

    [CliOption("--array-properties")]
    public string? ArrayProperties { get; set; }

    [CliOption("--depends-on")]
    public string[]? DependsOn { get; set; }

    [CliOption("--parameters")]
    public IEnumerable<KeyValue>? Parameters { get; set; }

    [CliOption("--container-overrides")]
    public string? ContainerOverrides { get; set; }

    [CliOption("--node-overrides")]
    public string? NodeOverrides { get; set; }

    [CliOption("--retry-strategy")]
    public string? RetryStrategy { get; set; }

    [CliOption("--timeout")]
    public string? Timeout { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--eks-properties-override")]
    public string? EksPropertiesOverride { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}