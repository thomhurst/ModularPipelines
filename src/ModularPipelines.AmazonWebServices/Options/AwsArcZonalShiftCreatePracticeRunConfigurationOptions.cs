using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("arc-zonal-shift", "create-practice-run-configuration")]
public record AwsArcZonalShiftCreatePracticeRunConfigurationOptions(
[property: CliOption("--outcome-alarms")] string[] OutcomeAlarms,
[property: CliOption("--resource-identifier")] string ResourceIdentifier
) : AwsOptions
{
    [CliOption("--blocked-dates")]
    public string[]? BlockedDates { get; set; }

    [CliOption("--blocked-windows")]
    public string[]? BlockedWindows { get; set; }

    [CliOption("--blocking-alarms")]
    public string[]? BlockingAlarms { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}