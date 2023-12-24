using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "create-subnet-cidr-reservation")]
public record AwsEc2CreateSubnetCidrReservationOptions(
[property: CommandSwitch("--subnet-id")] string SubnetId,
[property: CommandSwitch("--cidr")] string Cidr,
[property: CommandSwitch("--reservation-type")] string ReservationType
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--tag-specifications")]
    public string[]? TagSpecifications { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}