using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "create-job")]
public record AwsIotCreateJobOptions(
[property: CliOption("--job-id")] string JobId,
[property: CliOption("--targets")] string[] Targets
) : AwsOptions
{
    [CliOption("--document-source")]
    public string? DocumentSource { get; set; }

    [CliOption("--document")]
    public string? Document { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--presigned-url-config")]
    public string? PresignedUrlConfig { get; set; }

    [CliOption("--target-selection")]
    public string? TargetSelection { get; set; }

    [CliOption("--job-executions-rollout-config")]
    public string? JobExecutionsRolloutConfig { get; set; }

    [CliOption("--abort-config")]
    public string? AbortConfig { get; set; }

    [CliOption("--timeout-config")]
    public string? TimeoutConfig { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--namespace-id")]
    public string? NamespaceId { get; set; }

    [CliOption("--job-template-arn")]
    public string? JobTemplateArn { get; set; }

    [CliOption("--job-executions-retry-config")]
    public string? JobExecutionsRetryConfig { get; set; }

    [CliOption("--document-parameters")]
    public IEnumerable<KeyValue>? DocumentParameters { get; set; }

    [CliOption("--scheduling-config")]
    public string? SchedulingConfig { get; set; }

    [CliOption("--destination-package-versions")]
    public string[]? DestinationPackageVersions { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}