using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("batch", "register-job-definition")]
public record AwsBatchRegisterJobDefinitionOptions(
[property: CliOption("--job-definition-name")] string JobDefinitionName,
[property: CliOption("--type")] string Type
) : AwsOptions
{
    [CliOption("--parameters")]
    public IEnumerable<KeyValue>? Parameters { get; set; }

    [CliOption("--scheduling-priority")]
    public int? SchedulingPriority { get; set; }

    [CliOption("--container-properties")]
    public string? ContainerProperties { get; set; }

    [CliOption("--node-properties")]
    public string? NodeProperties { get; set; }

    [CliOption("--retry-strategy")]
    public string? RetryStrategy { get; set; }

    [CliOption("--timeout")]
    public string? Timeout { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--platform-capabilities")]
    public string[]? PlatformCapabilities { get; set; }

    [CliOption("--eks-properties")]
    public string? EksProperties { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}