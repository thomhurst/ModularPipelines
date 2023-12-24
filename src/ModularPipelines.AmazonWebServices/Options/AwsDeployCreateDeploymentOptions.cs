using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("deploy", "create-deployment")]
public record AwsDeployCreateDeploymentOptions(
[property: CommandSwitch("--application-name")] string ApplicationName
) : AwsOptions
{
    [CommandSwitch("--deployment-group-name")]
    public string? DeploymentGroupName { get; set; }

    [CommandSwitch("--revision")]
    public string? Revision { get; set; }

    [CommandSwitch("--deployment-config-name")]
    public string? DeploymentConfigName { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--target-instances")]
    public string? TargetInstances { get; set; }

    [CommandSwitch("--auto-rollback-configuration")]
    public string? AutoRollbackConfiguration { get; set; }

    [CommandSwitch("--file-exists-behavior")]
    public string? FileExistsBehavior { get; set; }

    [CommandSwitch("--override-alarm-configuration")]
    public string? OverrideAlarmConfiguration { get; set; }

    [CommandSwitch("--s3-location")]
    public string? S3Location { get; set; }

    [CommandSwitch("--github-location")]
    public string? GithubLocation { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}