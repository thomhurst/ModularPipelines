using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "create-subnet-cidr-reservation")]
public record AwsEc2CreateSubnetCidrReservationOptions(
[property: CliOption("--subnet-id")] string SubnetId,
[property: CliOption("--cidr")] string Cidr,
[property: CliOption("--reservation-type")] string ReservationType
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--tag-specifications")]
    public string[]? TagSpecifications { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}