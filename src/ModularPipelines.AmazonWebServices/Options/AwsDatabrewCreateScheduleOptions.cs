using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("databrew", "create-schedule")]
public record AwsDatabrewCreateScheduleOptions(
[property: CommandSwitch("--cron-expression")] string CronExpression,
[property: CommandSwitch("--name")] string Name
) : AwsOptions
{
    [CommandSwitch("--job-names")]
    public string[]? JobNames { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}