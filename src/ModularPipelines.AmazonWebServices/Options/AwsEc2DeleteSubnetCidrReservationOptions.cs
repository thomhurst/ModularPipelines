using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "delete-subnet-cidr-reservation")]
public record AwsEc2DeleteSubnetCidrReservationOptions(
[property: CliOption("--subnet-cidr-reservation-id")] string SubnetCidrReservationId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}