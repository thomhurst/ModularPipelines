using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("medialive", "update-reservation")]
public record AwsMedialiveUpdateReservationOptions(
[property: CliOption("--reservation-id")] string ReservationId
) : AwsOptions
{
    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--renewal-settings")]
    public string? RenewalSettings { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}