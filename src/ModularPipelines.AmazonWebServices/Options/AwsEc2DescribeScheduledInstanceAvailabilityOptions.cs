using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "describe-scheduled-instance-availability")]
public record AwsEc2DescribeScheduledInstanceAvailabilityOptions(
[property: CommandSwitch("--first-slot-start-time-range")] string FirstSlotStartTimeRange,
[property: CommandSwitch("--recurrence")] string Recurrence
) : AwsOptions
{
    [CommandSwitch("--filters")]
    public string[]? Filters { get; set; }

    [CommandSwitch("--max-slot-duration-in-hours")]
    public int? MaxSlotDurationInHours { get; set; }

    [CommandSwitch("--min-slot-duration-in-hours")]
    public int? MinSlotDurationInHours { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}