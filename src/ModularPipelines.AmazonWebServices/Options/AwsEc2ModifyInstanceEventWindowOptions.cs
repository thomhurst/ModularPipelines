using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "modify-instance-event-window")]
public record AwsEc2ModifyInstanceEventWindowOptions(
[property: CommandSwitch("--instance-event-window-id")] string InstanceEventWindowId
) : AwsOptions
{
    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--time-ranges")]
    public string[]? TimeRanges { get; set; }

    [CommandSwitch("--cron-expression")]
    public string? CronExpression { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}