using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "create-job")]
public record AwsIotCreateJobOptions(
[property: CommandSwitch("--job-id")] string JobId,
[property: CommandSwitch("--targets")] string[] Targets
) : AwsOptions
{
    [CommandSwitch("--document-source")]
    public string? DocumentSource { get; set; }

    [CommandSwitch("--document")]
    public string? Document { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--presigned-url-config")]
    public string? PresignedUrlConfig { get; set; }

    [CommandSwitch("--target-selection")]
    public string? TargetSelection { get; set; }

    [CommandSwitch("--job-executions-rollout-config")]
    public string? JobExecutionsRolloutConfig { get; set; }

    [CommandSwitch("--abort-config")]
    public string? AbortConfig { get; set; }

    [CommandSwitch("--timeout-config")]
    public string? TimeoutConfig { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--namespace-id")]
    public string? NamespaceId { get; set; }

    [CommandSwitch("--job-template-arn")]
    public string? JobTemplateArn { get; set; }

    [CommandSwitch("--job-executions-retry-config")]
    public string? JobExecutionsRetryConfig { get; set; }

    [CommandSwitch("--document-parameters")]
    public IEnumerable<KeyValue>? DocumentParameters { get; set; }

    [CommandSwitch("--scheduling-config")]
    public string? SchedulingConfig { get; set; }

    [CommandSwitch("--destination-package-versions")]
    public string[]? DestinationPackageVersions { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}