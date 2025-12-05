using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "modify-instance-event-window")]
public record AwsEc2ModifyInstanceEventWindowOptions(
[property: CliOption("--instance-event-window-id")] string InstanceEventWindowId
) : AwsOptions
{
    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--time-ranges")]
    public string[]? TimeRanges { get; set; }

    [CliOption("--cron-expression")]
    public string? CronExpression { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}