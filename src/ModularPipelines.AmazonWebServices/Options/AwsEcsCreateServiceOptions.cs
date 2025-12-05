using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ecs", "create-service")]
public record AwsEcsCreateServiceOptions(
[property: CliOption("--service-name")] string ServiceName
) : AwsOptions
{
    [CliOption("--cluster")]
    public string? Cluster { get; set; }

    [CliOption("--task-definition")]
    public string? TaskDefinition { get; set; }

    [CliOption("--load-balancers")]
    public string[]? LoadBalancers { get; set; }

    [CliOption("--service-registries")]
    public string[]? ServiceRegistries { get; set; }

    [CliOption("--desired-count")]
    public int? DesiredCount { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--launch-type")]
    public string? LaunchType { get; set; }

    [CliOption("--capacity-provider-strategy")]
    public string[]? CapacityProviderStrategy { get; set; }

    [CliOption("--platform-version")]
    public string? PlatformVersion { get; set; }

    [CliOption("--role")]
    public string? Role { get; set; }

    [CliOption("--deployment-configuration")]
    public string? DeploymentConfiguration { get; set; }

    [CliOption("--placement-constraints")]
    public string[]? PlacementConstraints { get; set; }

    [CliOption("--placement-strategy")]
    public string[]? PlacementStrategy { get; set; }

    [CliOption("--network-configuration")]
    public string? NetworkConfiguration { get; set; }

    [CliOption("--health-check-grace-period-seconds")]
    public int? HealthCheckGracePeriodSeconds { get; set; }

    [CliOption("--scheduling-strategy")]
    public string? SchedulingStrategy { get; set; }

    [CliOption("--deployment-controller")]
    public string? DeploymentController { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--propagate-tags")]
    public string? PropagateTags { get; set; }

    [CliOption("--service-connect-configuration")]
    public string? ServiceConnectConfiguration { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}