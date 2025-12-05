using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("autoscaling", "create-auto-scaling-group")]
public record AwsAutoscalingCreateAutoScalingGroupOptions(
[property: CliOption("--auto-scaling-group-name")] string AutoScalingGroupName,
[property: CliOption("--min-size")] int MinSize,
[property: CliOption("--max-size")] int MaxSize
) : AwsOptions
{
    [CliOption("--launch-configuration-name")]
    public string? LaunchConfigurationName { get; set; }

    [CliOption("--launch-template")]
    public string? LaunchTemplate { get; set; }

    [CliOption("--mixed-instances-policy")]
    public string? MixedInstancesPolicy { get; set; }

    [CliOption("--instance-id")]
    public string? InstanceId { get; set; }

    [CliOption("--desired-capacity")]
    public int? DesiredCapacity { get; set; }

    [CliOption("--default-cooldown")]
    public int? DefaultCooldown { get; set; }

    [CliOption("--availability-zones")]
    public string[]? AvailabilityZones { get; set; }

    [CliOption("--load-balancer-names")]
    public string[]? LoadBalancerNames { get; set; }

    [CliOption("--target-group-arns")]
    public string[]? TargetGroupArns { get; set; }

    [CliOption("--health-check-type")]
    public string? HealthCheckType { get; set; }

    [CliOption("--health-check-grace-period")]
    public int? HealthCheckGracePeriod { get; set; }

    [CliOption("--placement-group")]
    public string? PlacementGroup { get; set; }

    [CliOption("--vpc-zone-identifier")]
    public string? VpcZoneIdentifier { get; set; }

    [CliOption("--termination-policies")]
    public string[]? TerminationPolicies { get; set; }

    [CliOption("--lifecycle-hook-specification-list")]
    public string[]? LifecycleHookSpecificationList { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--service-linked-role-arn")]
    public string? ServiceLinkedRoleArn { get; set; }

    [CliOption("--max-instance-lifetime")]
    public int? MaxInstanceLifetime { get; set; }

    [CliOption("--context")]
    public string? Context { get; set; }

    [CliOption("--desired-capacity-type")]
    public string? DesiredCapacityType { get; set; }

    [CliOption("--default-instance-warmup")]
    public int? DefaultInstanceWarmup { get; set; }

    [CliOption("--traffic-sources")]
    public string[]? TrafficSources { get; set; }

    [CliOption("--instance-maintenance-policy")]
    public string? InstanceMaintenancePolicy { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}