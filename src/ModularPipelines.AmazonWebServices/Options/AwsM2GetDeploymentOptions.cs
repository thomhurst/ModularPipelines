using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("m2", "get-deployment")]
public record AwsM2GetDeploymentOptions(
[property: CommandSwitch("--application-id")] string ApplicationId,
[property: CommandSwitch("--deployment-id")] string DeploymentId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}