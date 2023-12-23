using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ecs", "create-service")]
public record AwsEcsCreateServiceOptions(
[property: CommandSwitch("--service-name")] string ServiceName
) : AwsOptions
{
    [CommandSwitch("--cluster")]
    public string? Cluster { get; set; }

    [CommandSwitch("--task-definition")]
    public string? TaskDefinition { get; set; }

    [CommandSwitch("--load-balancers")]
    public string[]? LoadBalancers { get; set; }

    [CommandSwitch("--service-registries")]
    public string[]? ServiceRegistries { get; set; }

    [CommandSwitch("--desired-count")]
    public int? DesiredCount { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--launch-type")]
    public string? LaunchType { get; set; }

    [CommandSwitch("--capacity-provider-strategy")]
    public string[]? CapacityProviderStrategy { get; set; }

    [CommandSwitch("--platform-version")]
    public string? PlatformVersion { get; set; }

    [CommandSwitch("--role")]
    public string? Role { get; set; }

    [CommandSwitch("--deployment-configuration")]
    public string? DeploymentConfiguration { get; set; }

    [CommandSwitch("--placement-constraints")]
    public string[]? PlacementConstraints { get; set; }

    [CommandSwitch("--placement-strategy")]
    public string[]? PlacementStrategy { get; set; }

    [CommandSwitch("--network-configuration")]
    public string? NetworkConfiguration { get; set; }

    [CommandSwitch("--health-check-grace-period-seconds")]
    public int? HealthCheckGracePeriodSeconds { get; set; }

    [CommandSwitch("--scheduling-strategy")]
    public string? SchedulingStrategy { get; set; }

    [CommandSwitch("--deployment-controller")]
    public string? DeploymentController { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--propagate-tags")]
    public string? PropagateTags { get; set; }

    [CommandSwitch("--service-connect-configuration")]
    public string? ServiceConnectConfiguration { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}