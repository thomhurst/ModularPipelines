using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "create-job-template")]
public record AwsIotCreateJobTemplateOptions(
[property: CliOption("--job-template-id")] string JobTemplateId,
[property: CliOption("--description")] string Description
) : AwsOptions
{
    [CliOption("--job-arn")]
    public string? JobArn { get; set; }

    [CliOption("--document-source")]
    public string? DocumentSource { get; set; }

    [CliOption("--document")]
    public string? Document { get; set; }

    [CliOption("--presigned-url-config")]
    public string? PresignedUrlConfig { get; set; }

    [CliOption("--job-executions-rollout-config")]
    public string? JobExecutionsRolloutConfig { get; set; }

    [CliOption("--abort-config")]
    public string? AbortConfig { get; set; }

    [CliOption("--timeout-config")]
    public string? TimeoutConfig { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--job-executions-retry-config")]
    public string? JobExecutionsRetryConfig { get; set; }

    [CliOption("--maintenance-windows")]
    public string[]? MaintenanceWindows { get; set; }

    [CliOption("--destination-package-versions")]
    public string[]? DestinationPackageVersions { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}