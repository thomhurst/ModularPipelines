using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "describe-scheduled-instance-availability")]
public record AwsEc2DescribeScheduledInstanceAvailabilityOptions(
[property: CliOption("--first-slot-start-time-range")] string FirstSlotStartTimeRange,
[property: CliOption("--recurrence")] string Recurrence
) : AwsOptions
{
    [CliOption("--filters")]
    public string[]? Filters { get; set; }

    [CliOption("--max-slot-duration-in-hours")]
    public int? MaxSlotDurationInHours { get; set; }

    [CliOption("--min-slot-duration-in-hours")]
    public int? MinSlotDurationInHours { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}