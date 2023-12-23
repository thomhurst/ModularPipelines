using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ssm", "get-calendar-state")]
public record AwsSsmGetCalendarStateOptions(
[property: CommandSwitch("--calendar-names")] string[] CalendarNames
) : AwsOptions
{
    [CommandSwitch("--at-time")]
    public string? AtTime { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}