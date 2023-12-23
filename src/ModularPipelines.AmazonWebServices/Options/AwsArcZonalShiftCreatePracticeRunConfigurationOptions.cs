using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("arc-zonal-shift", "create-practice-run-configuration")]
public record AwsArcZonalShiftCreatePracticeRunConfigurationOptions(
[property: CommandSwitch("--outcome-alarms")] string[] OutcomeAlarms,
[property: CommandSwitch("--resource-identifier")] string ResourceIdentifier
) : AwsOptions
{
    [CommandSwitch("--blocked-dates")]
    public string[]? BlockedDates { get; set; }

    [CommandSwitch("--blocked-windows")]
    public string[]? BlockedWindows { get; set; }

    [CommandSwitch("--blocking-alarms")]
    public string[]? BlockingAlarms { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}