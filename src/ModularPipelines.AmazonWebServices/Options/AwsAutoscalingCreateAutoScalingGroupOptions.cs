using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("autoscaling", "create-auto-scaling-group")]
public record AwsAutoscalingCreateAutoScalingGroupOptions(
[property: CommandSwitch("--auto-scaling-group-name")] string AutoScalingGroupName,
[property: CommandSwitch("--min-size")] int MinSize,
[property: CommandSwitch("--max-size")] int MaxSize
) : AwsOptions
{
    [CommandSwitch("--launch-configuration-name")]
    public string? LaunchConfigurationName { get; set; }

    [CommandSwitch("--launch-template")]
    public string? LaunchTemplate { get; set; }

    [CommandSwitch("--mixed-instances-policy")]
    public string? MixedInstancesPolicy { get; set; }

    [CommandSwitch("--instance-id")]
    public string? InstanceId { get; set; }

    [CommandSwitch("--desired-capacity")]
    public int? DesiredCapacity { get; set; }

    [CommandSwitch("--default-cooldown")]
    public int? DefaultCooldown { get; set; }

    [CommandSwitch("--availability-zones")]
    public string[]? AvailabilityZones { get; set; }

    [CommandSwitch("--load-balancer-names")]
    public string[]? LoadBalancerNames { get; set; }

    [CommandSwitch("--target-group-arns")]
    public string[]? TargetGroupArns { get; set; }

    [CommandSwitch("--health-check-type")]
    public string? HealthCheckType { get; set; }

    [CommandSwitch("--health-check-grace-period")]
    public int? HealthCheckGracePeriod { get; set; }

    [CommandSwitch("--placement-group")]
    public string? PlacementGroup { get; set; }

    [CommandSwitch("--vpc-zone-identifier")]
    public string? VpcZoneIdentifier { get; set; }

    [CommandSwitch("--termination-policies")]
    public string[]? TerminationPolicies { get; set; }

    [CommandSwitch("--lifecycle-hook-specification-list")]
    public string[]? LifecycleHookSpecificationList { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--service-linked-role-arn")]
    public string? ServiceLinkedRoleArn { get; set; }

    [CommandSwitch("--max-instance-lifetime")]
    public int? MaxInstanceLifetime { get; set; }

    [CommandSwitch("--context")]
    public string? Context { get; set; }

    [CommandSwitch("--desired-capacity-type")]
    public string? DesiredCapacityType { get; set; }

    [CommandSwitch("--default-instance-warmup")]
    public int? DefaultInstanceWarmup { get; set; }

    [CommandSwitch("--traffic-sources")]
    public string[]? TrafficSources { get; set; }

    [CommandSwitch("--instance-maintenance-policy")]
    public string? InstanceMaintenancePolicy { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}