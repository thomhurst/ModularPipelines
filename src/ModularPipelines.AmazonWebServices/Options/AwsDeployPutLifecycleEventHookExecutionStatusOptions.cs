using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("deploy", "put-lifecycle-event-hook-execution-status")]
public record AwsDeployPutLifecycleEventHookExecutionStatusOptions : AwsOptions
{
    [CommandSwitch("--deployment-id")]
    public string? DeploymentId { get; set; }

    [CommandSwitch("--lifecycle-event-hook-execution-id")]
    public string? LifecycleEventHookExecutionId { get; set; }

    [CommandSwitch("--status")]
    public string? Status { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}