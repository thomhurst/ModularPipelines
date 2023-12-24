using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("deploy", "get-deployment-target")]
public record AwsDeployGetDeploymentTargetOptions(
[property: CommandSwitch("--deployment-id")] string DeploymentId,
[property: CommandSwitch("--target-id")] string TargetId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}