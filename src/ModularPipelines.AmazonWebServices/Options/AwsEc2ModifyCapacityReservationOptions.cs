using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "modify-capacity-reservation")]
public record AwsEc2ModifyCapacityReservationOptions(
[property: CliOption("--capacity-reservation-id")] string CapacityReservationId
) : AwsOptions
{
    [CliOption("--instance-count")]
    public int? InstanceCount { get; set; }

    [CliOption("--end-date")]
    public long? EndDate { get; set; }

    [CliOption("--end-date-type")]
    public string? EndDateType { get; set; }

    [CliOption("--additional-info")]
    public string? AdditionalInfo { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}