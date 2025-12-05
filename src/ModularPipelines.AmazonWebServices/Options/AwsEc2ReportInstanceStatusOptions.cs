using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "report-instance-status")]
public record AwsEc2ReportInstanceStatusOptions(
[property: CliOption("--instances")] string[] Instances,
[property: CliOption("--reason-codes")] string[] ReasonCodes,
[property: CliOption("--status")] string Status
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--end-time")]
    public long? EndTime { get; set; }

    [CliOption("--start-time")]
    public long? StartTime { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}