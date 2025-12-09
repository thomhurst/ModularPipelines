using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("backup-gateway", "put-maintenance-start-time")]
public record AwsBackupGatewayPutMaintenanceStartTimeOptions(
[property: CliOption("--gateway-arn")] string GatewayArn,
[property: CliOption("--hour-of-day")] int HourOfDay,
[property: CliOption("--minute-of-hour")] int MinuteOfHour
) : AwsOptions
{
    [CliOption("--day-of-month")]
    public int? DayOfMonth { get; set; }

    [CliOption("--day-of-week")]
    public int? DayOfWeek { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}