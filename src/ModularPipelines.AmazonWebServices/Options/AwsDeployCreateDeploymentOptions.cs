using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("deploy", "create-deployment")]
public record AwsDeployCreateDeploymentOptions(
[property: CliOption("--application-name")] string ApplicationName
) : AwsOptions
{
    [CliOption("--deployment-group-name")]
    public string? DeploymentGroupName { get; set; }

    [CliOption("--revision")]
    public string? Revision { get; set; }

    [CliOption("--deployment-config-name")]
    public string? DeploymentConfigName { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--target-instances")]
    public string? TargetInstances { get; set; }

    [CliOption("--auto-rollback-configuration")]
    public string? AutoRollbackConfiguration { get; set; }

    [CliOption("--file-exists-behavior")]
    public string? FileExistsBehavior { get; set; }

    [CliOption("--override-alarm-configuration")]
    public string? OverrideAlarmConfiguration { get; set; }

    [CliOption("--s3-location")]
    public string? S3Location { get; set; }

    [CliOption("--github-location")]
    public string? GithubLocation { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}