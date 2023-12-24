using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "describe-capacity-block-offerings")]
public record AwsEc2DescribeCapacityBlockOfferingsOptions(
[property: CommandSwitch("--instance-type")] string InstanceType,
[property: CommandSwitch("--instance-count")] int InstanceCount,
[property: CommandSwitch("--capacity-duration-hours")] int CapacityDurationHours
) : AwsOptions
{
    [CommandSwitch("--start-date-range")]
    public long? StartDateRange { get; set; }

    [CommandSwitch("--end-date-range")]
    public long? EndDateRange { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}