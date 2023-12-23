using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("databrew", "update-schedule")]
public record AwsDatabrewUpdateScheduleOptions(
[property: CommandSwitch("--cron-expression")] string CronExpression,
[property: CommandSwitch("--name")] string Name
) : AwsOptions
{
    [CommandSwitch("--job-names")]
    public string[]? JobNames { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}