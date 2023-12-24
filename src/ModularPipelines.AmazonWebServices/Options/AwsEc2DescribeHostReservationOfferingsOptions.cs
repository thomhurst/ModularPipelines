using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "describe-host-reservation-offerings")]
public record AwsEc2DescribeHostReservationOfferingsOptions : AwsOptions
{
    [CommandSwitch("--filter")]
    public string[]? Filter { get; set; }

    [CommandSwitch("--max-duration")]
    public int? MaxDuration { get; set; }

    [CommandSwitch("--min-duration")]
    public int? MinDuration { get; set; }

    [CommandSwitch("--offering-id")]
    public string? OfferingId { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}