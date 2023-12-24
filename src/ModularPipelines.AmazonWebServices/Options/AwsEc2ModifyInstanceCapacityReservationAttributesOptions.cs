using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "modify-instance-capacity-reservation-attributes")]
public record AwsEc2ModifyInstanceCapacityReservationAttributesOptions(
[property: CommandSwitch("--instance-id")] string InstanceId,
[property: CommandSwitch("--capacity-reservation-specification")] string CapacityReservationSpecification
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}