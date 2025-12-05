using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("storagegateway", "update-maintenance-start-time")]
public record AwsStoragegatewayUpdateMaintenanceStartTimeOptions(
[property: CliOption("--gateway-arn")] string GatewayArn,
[property: CliOption("--hour-of-day")] int HourOfDay,
[property: CliOption("--minute-of-hour")] int MinuteOfHour
) : AwsOptions
{
    [CliOption("--day-of-week")]
    public int? DayOfWeek { get; set; }

    [CliOption("--day-of-month")]
    public int? DayOfMonth { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}