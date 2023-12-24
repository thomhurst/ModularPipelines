using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("batch", "submit-job")]
public record AwsBatchSubmitJobOptions(
[property: CommandSwitch("--job-name")] string JobName,
[property: CommandSwitch("--job-queue")] string JobQueue,
[property: CommandSwitch("--job-definition")] string JobDefinition
) : AwsOptions
{
    [CommandSwitch("--share-identifier")]
    public string? ShareIdentifier { get; set; }

    [CommandSwitch("--scheduling-priority-override")]
    public int? SchedulingPriorityOverride { get; set; }

    [CommandSwitch("--array-properties")]
    public string? ArrayProperties { get; set; }

    [CommandSwitch("--depends-on")]
    public string[]? DependsOn { get; set; }

    [CommandSwitch("--parameters")]
    public IEnumerable<KeyValue>? Parameters { get; set; }

    [CommandSwitch("--container-overrides")]
    public string? ContainerOverrides { get; set; }

    [CommandSwitch("--node-overrides")]
    public string? NodeOverrides { get; set; }

    [CommandSwitch("--retry-strategy")]
    public string? RetryStrategy { get; set; }

    [CommandSwitch("--timeout")]
    public string? Timeout { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--eks-properties-override")]
    public string? EksPropertiesOverride { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}