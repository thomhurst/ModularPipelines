using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("deploy", "get-deployment-group")]
public record AwsDeployGetDeploymentGroupOptions(
[property: CommandSwitch("--application-name")] string ApplicationName,
[property: CommandSwitch("--deployment-group-name")] string DeploymentGroupName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}