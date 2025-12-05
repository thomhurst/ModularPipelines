using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mgh", "notify-migration-task-state")]
public record AwsMghNotifyMigrationTaskStateOptions(
[property: CliOption("--progress-update-stream")] string ProgressUpdateStream,
[property: CliOption("--migration-task-name")] string MigrationTaskName,
[property: CliOption("--task")] string Task,
[property: CliOption("--update-date-time")] long UpdateDateTime,
[property: CliOption("--next-update-seconds")] int NextUpdateSeconds
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}