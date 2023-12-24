using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("scheduler", "create-schedule")]
public record AwsSchedulerCreateScheduleOptions(
[property: CommandSwitch("--flexible-time-window")] string FlexibleTimeWindow,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--schedule-expression")] string ScheduleExpression,
[property: CommandSwitch("--target")] string Target
) : AwsOptions
{
    [CommandSwitch("--action-after-completion")]
    public string? ActionAfterCompletion { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--end-date")]
    public long? EndDate { get; set; }

    [CommandSwitch("--group-name")]
    public string? GroupName { get; set; }

    [CommandSwitch("--kms-key-arn")]
    public string? KmsKeyArn { get; set; }

    [CommandSwitch("--schedule-expression-timezone")]
    public string? ScheduleExpressionTimezone { get; set; }

    [CommandSwitch("--start-date")]
    public long? StartDate { get; set; }

    [CommandSwitch("--state")]
    public string? State { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}