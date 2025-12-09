using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("deploy", "create-deployment-group")]
public record AwsDeployCreateDeploymentGroupOptions(
[property: CliOption("--application-name")] string ApplicationName,
[property: CliOption("--deployment-group-name")] string DeploymentGroupName,
[property: CliOption("--service-role-arn")] string ServiceRoleArn
) : AwsOptions
{
    [CliOption("--deployment-config-name")]
    public string? DeploymentConfigName { get; set; }

    [CliOption("--ec2-tag-filters")]
    public string[]? Ec2TagFilters { get; set; }

    [CliOption("--on-premises-instance-tag-filters")]
    public string[]? OnPremisesInstanceTagFilters { get; set; }

    [CliOption("--auto-scaling-groups")]
    public string[]? AutoScalingGroups { get; set; }

    [CliOption("--trigger-configurations")]
    public string[]? TriggerConfigurations { get; set; }

    [CliOption("--alarm-configuration")]
    public string? AlarmConfiguration { get; set; }

    [CliOption("--auto-rollback-configuration")]
    public string? AutoRollbackConfiguration { get; set; }

    [CliOption("--outdated-instances-strategy")]
    public string? OutdatedInstancesStrategy { get; set; }

    [CliOption("--deployment-style")]
    public string? DeploymentStyle { get; set; }

    [CliOption("--blue-green-deployment-configuration")]
    public string? BlueGreenDeploymentConfiguration { get; set; }

    [CliOption("--load-balancer-info")]
    public string? LoadBalancerInfo { get; set; }

    [CliOption("--ec2-tag-set")]
    public string? Ec2TagSet { get; set; }

    [CliOption("--ecs-services")]
    public string[]? EcsServices { get; set; }

    [CliOption("--on-premises-tag-set")]
    public string? OnPremisesTagSet { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}