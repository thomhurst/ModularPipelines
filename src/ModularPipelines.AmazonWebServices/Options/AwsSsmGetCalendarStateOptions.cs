using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ssm", "get-calendar-state")]
public record AwsSsmGetCalendarStateOptions(
[property: CliOption("--calendar-names")] string[] CalendarNames
) : AwsOptions
{
    [CliOption("--at-time")]
    public string? AtTime { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}