using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("backup-gateway", "put-maintenance-start-time")]
public record AwsBackupGatewayPutMaintenanceStartTimeOptions(
[property: CommandSwitch("--gateway-arn")] string GatewayArn,
[property: CommandSwitch("--hour-of-day")] int HourOfDay,
[property: CommandSwitch("--minute-of-hour")] int MinuteOfHour
) : AwsOptions
{
    [CommandSwitch("--day-of-month")]
    public int? DayOfMonth { get; set; }

    [CommandSwitch("--day-of-week")]
    public int? DayOfWeek { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}