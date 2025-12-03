using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("timestream-query", "create-scheduled-query")]
public record AwsTimestreamQueryCreateScheduledQueryOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--query-string")] string QueryString,
[property: CliOption("--schedule-configuration")] string ScheduleConfiguration,
[property: CliOption("--notification-configuration")] string NotificationConfiguration,
[property: CliOption("--scheduled-query-execution-role-arn")] string ScheduledQueryExecutionRoleArn,
[property: CliOption("--error-report-configuration")] string ErrorReportConfiguration
) : AwsOptions
{
    [CliOption("--target-configuration")]
    public string? TargetConfiguration { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--kms-key-id")]
    public string? KmsKeyId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}