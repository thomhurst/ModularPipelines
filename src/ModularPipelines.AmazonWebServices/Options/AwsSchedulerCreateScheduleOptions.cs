using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("scheduler", "create-schedule")]
public record AwsSchedulerCreateScheduleOptions(
[property: CliOption("--flexible-time-window")] string FlexibleTimeWindow,
[property: CliOption("--name")] string Name,
[property: CliOption("--schedule-expression")] string ScheduleExpression,
[property: CliOption("--target")] string Target
) : AwsOptions
{
    [CliOption("--action-after-completion")]
    public string? ActionAfterCompletion { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--end-date")]
    public long? EndDate { get; set; }

    [CliOption("--group-name")]
    public string? GroupName { get; set; }

    [CliOption("--kms-key-arn")]
    public string? KmsKeyArn { get; set; }

    [CliOption("--schedule-expression-timezone")]
    public string? ScheduleExpressionTimezone { get; set; }

    [CliOption("--start-date")]
    public long? StartDate { get; set; }

    [CliOption("--state")]
    public string? State { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}