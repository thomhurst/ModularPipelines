using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("deploy", "batch-get-deployment-targets")]
public record AwsDeployBatchGetDeploymentTargetsOptions(
[property: CommandSwitch("--deployment-id")] string DeploymentId,
[property: CommandSwitch("--target-ids")] string[] TargetIds
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}