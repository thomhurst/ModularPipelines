using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "create-job-template")]
public record AwsIotCreateJobTemplateOptions(
[property: CommandSwitch("--job-template-id")] string JobTemplateId,
[property: CommandSwitch("--description")] string Description
) : AwsOptions
{
    [CommandSwitch("--job-arn")]
    public string? JobArn { get; set; }

    [CommandSwitch("--document-source")]
    public string? DocumentSource { get; set; }

    [CommandSwitch("--document")]
    public string? Document { get; set; }

    [CommandSwitch("--presigned-url-config")]
    public string? PresignedUrlConfig { get; set; }

    [CommandSwitch("--job-executions-rollout-config")]
    public string? JobExecutionsRolloutConfig { get; set; }

    [CommandSwitch("--abort-config")]
    public string? AbortConfig { get; set; }

    [CommandSwitch("--timeout-config")]
    public string? TimeoutConfig { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--job-executions-retry-config")]
    public string? JobExecutionsRetryConfig { get; set; }

    [CommandSwitch("--maintenance-windows")]
    public string[]? MaintenanceWindows { get; set; }

    [CommandSwitch("--destination-package-versions")]
    public string[]? DestinationPackageVersions { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}