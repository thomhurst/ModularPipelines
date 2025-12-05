using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "describe-host-reservation-offerings")]
public record AwsEc2DescribeHostReservationOfferingsOptions : AwsOptions
{
    [CliOption("--filter")]
    public string[]? Filter { get; set; }

    [CliOption("--max-duration")]
    public int? MaxDuration { get; set; }

    [CliOption("--min-duration")]
    public int? MinDuration { get; set; }

    [CliOption("--offering-id")]
    public string? OfferingId { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}