using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "modify-capacity-reservation")]
public record AwsEc2ModifyCapacityReservationOptions(
[property: CommandSwitch("--capacity-reservation-id")] string CapacityReservationId
) : AwsOptions
{
    [CommandSwitch("--instance-count")]
    public int? InstanceCount { get; set; }

    [CommandSwitch("--end-date")]
    public long? EndDate { get; set; }

    [CommandSwitch("--end-date-type")]
    public string? EndDateType { get; set; }

    [CommandSwitch("--additional-info")]
    public string? AdditionalInfo { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}