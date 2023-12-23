using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ecs", "update-service")]
public record AwsEcsUpdateServiceOptions(
[property: CommandSwitch("--service")] string Service
) : AwsOptions
{
    [CommandSwitch("--cluster")]
    public string? Cluster { get; set; }

    [CommandSwitch("--desired-count")]
    public int? DesiredCount { get; set; }

    [CommandSwitch("--task-definition")]
    public string? TaskDefinition { get; set; }

    [CommandSwitch("--capacity-provider-strategy")]
    public string[]? CapacityProviderStrategy { get; set; }

    [CommandSwitch("--deployment-configuration")]
    public string? DeploymentConfiguration { get; set; }

    [CommandSwitch("--network-configuration")]
    public string? NetworkConfiguration { get; set; }

    [CommandSwitch("--placement-constraints")]
    public string[]? PlacementConstraints { get; set; }

    [CommandSwitch("--placement-strategy")]
    public string[]? PlacementStrategy { get; set; }

    [CommandSwitch("--platform-version")]
    public string? PlatformVersion { get; set; }

    [CommandSwitch("--health-check-grace-period-seconds")]
    public int? HealthCheckGracePeriodSeconds { get; set; }

    [CommandSwitch("--load-balancers")]
    public string[]? LoadBalancers { get; set; }

    [CommandSwitch("--propagate-tags")]
    public string? PropagateTags { get; set; }

    [CommandSwitch("--service-registries")]
    public string[]? ServiceRegistries { get; set; }

    [CommandSwitch("--service-connect-configuration")]
    public string? ServiceConnectConfiguration { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}