using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "create-capacity-reservation")]
public record AwsEc2CreateCapacityReservationOptions(
[property: CliOption("--instance-type")] string InstanceType,
[property: CliOption("--instance-platform")] string InstancePlatform,
[property: CliOption("--instance-count")] int InstanceCount
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--availability-zone")]
    public string? AvailabilityZone { get; set; }

    [CliOption("--availability-zone-id")]
    public string? AvailabilityZoneId { get; set; }

    [CliOption("--tenancy")]
    public string? Tenancy { get; set; }

    [CliOption("--end-date")]
    public long? EndDate { get; set; }

    [CliOption("--end-date-type")]
    public string? EndDateType { get; set; }

    [CliOption("--instance-match-criteria")]
    public string? InstanceMatchCriteria { get; set; }

    [CliOption("--tag-specifications")]
    public string[]? TagSpecifications { get; set; }

    [CliOption("--outpost-arn")]
    public string? OutpostArn { get; set; }

    [CliOption("--placement-group-arn")]
    public string? PlacementGroupArn { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}