using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("autoscaling", "update-auto-scaling-group")]
public record AwsAutoscalingUpdateAutoScalingGroupOptions(
[property: CommandSwitch("--auto-scaling-group-name")] string AutoScalingGroupName
) : AwsOptions
{
    [CommandSwitch("--launch-configuration-name")]
    public string? LaunchConfigurationName { get; set; }

    [CommandSwitch("--launch-template")]
    public string? LaunchTemplate { get; set; }

    [CommandSwitch("--mixed-instances-policy")]
    public string? MixedInstancesPolicy { get; set; }

    [CommandSwitch("--min-size")]
    public int? MinSize { get; set; }

    [CommandSwitch("--max-size")]
    public int? MaxSize { get; set; }

    [CommandSwitch("--desired-capacity")]
    public int? DesiredCapacity { get; set; }

    [CommandSwitch("--default-cooldown")]
    public int? DefaultCooldown { get; set; }

    [CommandSwitch("--availability-zones")]
    public string[]? AvailabilityZones { get; set; }

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

    [CommandSwitch("--instance-maintenance-policy")]
    public string? InstanceMaintenancePolicy { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}