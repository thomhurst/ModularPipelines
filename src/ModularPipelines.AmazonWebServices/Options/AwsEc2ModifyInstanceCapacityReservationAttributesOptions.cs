using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "modify-instance-capacity-reservation-attributes")]
public record AwsEc2ModifyInstanceCapacityReservationAttributesOptions(
[property: CliOption("--instance-id")] string InstanceId,
[property: CliOption("--capacity-reservation-specification")] string CapacityReservationSpecification
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}