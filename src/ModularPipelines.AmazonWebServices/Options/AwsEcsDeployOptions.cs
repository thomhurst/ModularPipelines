using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ecs", "deploy")]
public record AwsEcsDeployOptions(
[property: CliOption("--service")] string Service,
[property: CliOption("--task-definition")] string TaskDefinition,
[property: CliOption("--codedeploy-appspec")] string CodedeployAppspec
) : AwsOptions
{
    [CliOption("--cluster")]
    public string? Cluster { get; set; }

    [CliOption("--codedeploy-application")]
    public string? CodedeployApplication { get; set; }

    [CliOption("--codedeploy-deployment-group")]
    public string? CodedeployDeploymentGroup { get; set; }
}