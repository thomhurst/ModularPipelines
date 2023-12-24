using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("timestream-query", "create-scheduled-query")]
public record AwsTimestreamQueryCreateScheduledQueryOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--query-string")] string QueryString,
[property: CommandSwitch("--schedule-configuration")] string ScheduleConfiguration,
[property: CommandSwitch("--notification-configuration")] string NotificationConfiguration,
[property: CommandSwitch("--scheduled-query-execution-role-arn")] string ScheduledQueryExecutionRoleArn,
[property: CommandSwitch("--error-report-configuration")] string ErrorReportConfiguration
) : AwsOptions
{
    [CommandSwitch("--target-configuration")]
    public string? TargetConfiguration { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--kms-key-id")]
    public string? KmsKeyId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}