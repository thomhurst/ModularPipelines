using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "create-capacity-reservation-fleet")]
public record AwsEc2CreateCapacityReservationFleetOptions(
[property: CommandSwitch("--instance-type-specifications")] string[] InstanceTypeSpecifications,
[property: CommandSwitch("--total-target-capacity")] int TotalTargetCapacity
) : AwsOptions
{
    [CommandSwitch("--allocation-strategy")]
    public string? AllocationStrategy { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--tenancy")]
    public string? Tenancy { get; set; }

    [CommandSwitch("--end-date")]
    public long? EndDate { get; set; }

    [CommandSwitch("--instance-match-criteria")]
    public string? InstanceMatchCriteria { get; set; }

    [CommandSwitch("--tag-specifications")]
    public string[]? TagSpecifications { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}