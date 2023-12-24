using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "delete-subnet-cidr-reservation")]
public record AwsEc2DeleteSubnetCidrReservationOptions(
[property: CommandSwitch("--subnet-cidr-reservation-id")] string SubnetCidrReservationId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}