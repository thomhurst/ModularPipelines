using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("batch", "register-job-definition")]
public record AwsBatchRegisterJobDefinitionOptions(
[property: CommandSwitch("--job-definition-name")] string JobDefinitionName,
[property: CommandSwitch("--type")] string Type
) : AwsOptions
{
    [CommandSwitch("--parameters")]
    public IEnumerable<KeyValue>? Parameters { get; set; }

    [CommandSwitch("--scheduling-priority")]
    public int? SchedulingPriority { get; set; }

    [CommandSwitch("--container-properties")]
    public string? ContainerProperties { get; set; }

    [CommandSwitch("--node-properties")]
    public string? NodeProperties { get; set; }

    [CommandSwitch("--retry-strategy")]
    public string? RetryStrategy { get; set; }

    [CommandSwitch("--timeout")]
    public string? Timeout { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--platform-capabilities")]
    public string[]? PlatformCapabilities { get; set; }

    [CommandSwitch("--eks-properties")]
    public string? EksProperties { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}