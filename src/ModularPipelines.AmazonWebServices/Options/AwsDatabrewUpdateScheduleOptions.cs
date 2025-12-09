using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("databrew", "update-schedule")]
public record AwsDatabrewUpdateScheduleOptions(
[property: CliOption("--cron-expression")] string CronExpression,
[property: CliOption("--name")] string Name
) : AwsOptions
{
    [CliOption("--job-names")]
    public string[]? JobNames { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}