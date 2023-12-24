using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("redshift-serverless", "create-scheduled-action")]
public record AwsRedshiftServerlessCreateScheduledActionOptions(
[property: CommandSwitch("--namespace-name")] string NamespaceName,
[property: CommandSwitch("--role-arn")] string RoleArn,
[property: CommandSwitch("--schedule")] string Schedule,
[property: CommandSwitch("--scheduled-action-name")] string ScheduledActionName,
[property: CommandSwitch("--target-action")] string TargetAction
) : AwsOptions
{
    [CommandSwitch("--end-time")]
    public long? EndTime { get; set; }

    [CommandSwitch("--scheduled-action-description")]
    public string? ScheduledActionDescription { get; set; }

    [CommandSwitch("--start-time")]
    public long? StartTime { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}