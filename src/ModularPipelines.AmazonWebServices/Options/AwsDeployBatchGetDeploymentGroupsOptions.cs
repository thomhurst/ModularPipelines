using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("deploy", "batch-get-deployment-groups")]
public record AwsDeployBatchGetDeploymentGroupsOptions(
[property: CommandSwitch("--application-name")] string ApplicationName,
[property: CommandSwitch("--deployment-group-names")] string[] DeploymentGroupNames
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}