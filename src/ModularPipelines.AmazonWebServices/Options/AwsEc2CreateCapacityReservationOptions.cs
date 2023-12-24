using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "create-capacity-reservation")]
public record AwsEc2CreateCapacityReservationOptions(
[property: CommandSwitch("--instance-type")] string InstanceType,
[property: CommandSwitch("--instance-platform")] string InstancePlatform,
[property: CommandSwitch("--instance-count")] int InstanceCount
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--availability-zone")]
    public string? AvailabilityZone { get; set; }

    [CommandSwitch("--availability-zone-id")]
    public string? AvailabilityZoneId { get; set; }

    [CommandSwitch("--tenancy")]
    public string? Tenancy { get; set; }

    [CommandSwitch("--end-date")]
    public long? EndDate { get; set; }

    [CommandSwitch("--end-date-type")]
    public string? EndDateType { get; set; }

    [CommandSwitch("--instance-match-criteria")]
    public string? InstanceMatchCriteria { get; set; }

    [CommandSwitch("--tag-specifications")]
    public string[]? TagSpecifications { get; set; }

    [CommandSwitch("--outpost-arn")]
    public string? OutpostArn { get; set; }

    [CommandSwitch("--placement-group-arn")]
    public string? PlacementGroupArn { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}