using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "describe-host-reservations")]
public record AwsEc2DescribeHostReservationsOptions : AwsOptions
{
    [CliOption("--filter")]
    public string[]? Filter { get; set; }

    [CliOption("--host-reservation-id-set")]
    public string[]? HostReservationIdSet { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}