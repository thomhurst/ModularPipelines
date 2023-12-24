using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("redshift-serverless", "update-scheduled-action")]
public record AwsRedshiftServerlessUpdateScheduledActionOptions(
[property: CommandSwitch("--scheduled-action-name")] string ScheduledActionName
) : AwsOptions
{
    [CommandSwitch("--end-time")]
    public long? EndTime { get; set; }

    [CommandSwitch("--role-arn")]
    public string? RoleArn { get; set; }

    [CommandSwitch("--schedule")]
    public string? Schedule { get; set; }

    [CommandSwitch("--scheduled-action-description")]
    public string? ScheduledActionDescription { get; set; }

    [CommandSwitch("--start-time")]
    public long? StartTime { get; set; }

    [CommandSwitch("--target-action")]
    public string? TargetAction { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}