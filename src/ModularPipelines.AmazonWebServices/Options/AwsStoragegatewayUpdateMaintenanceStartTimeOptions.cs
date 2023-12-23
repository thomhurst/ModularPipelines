using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storagegateway", "update-maintenance-start-time")]
public record AwsStoragegatewayUpdateMaintenanceStartTimeOptions(
[property: CommandSwitch("--gateway-arn")] string GatewayArn,
[property: CommandSwitch("--hour-of-day")] int HourOfDay,
[property: CommandSwitch("--minute-of-hour")] int MinuteOfHour
) : AwsOptions
{
    [CommandSwitch("--day-of-week")]
    public int? DayOfWeek { get; set; }

    [CommandSwitch("--day-of-month")]
    public int? DayOfMonth { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}