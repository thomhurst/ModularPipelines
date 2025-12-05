using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("deploy", "put-lifecycle-event-hook-execution-status")]
public record AwsDeployPutLifecycleEventHookExecutionStatusOptions : AwsOptions
{
    [CliOption("--deployment-id")]
    public string? DeploymentId { get; set; }

    [CliOption("--lifecycle-event-hook-execution-id")]
    public string? LifecycleEventHookExecutionId { get; set; }

    [CliOption("--status")]
    public string? Status { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}