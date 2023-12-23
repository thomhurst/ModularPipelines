using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("appconfig", "get-deployment")]
public record AwsAppconfigGetDeploymentOptions(
[property: CommandSwitch("--application-id")] string ApplicationId,
[property: CommandSwitch("--environment-id")] string EnvironmentId,
[property: CommandSwitch("--deployment-number")] int DeploymentNumber
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}