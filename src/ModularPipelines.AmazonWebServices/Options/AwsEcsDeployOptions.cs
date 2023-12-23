using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ecs", "deploy")]
public record AwsEcsDeployOptions(
[property: CommandSwitch("--service")] string Service,
[property: CommandSwitch("--task-definition")] string TaskDefinition,
[property: CommandSwitch("--codedeploy-appspec")] string CodedeployAppspec
) : AwsOptions
{
    [CommandSwitch("--cluster")]
    public string? Cluster { get; set; }

    [CommandSwitch("--codedeploy-application")]
    public string? CodedeployApplication { get; set; }

    [CommandSwitch("--codedeploy-deployment-group")]
    public string? CodedeployDeploymentGroup { get; set; }
}