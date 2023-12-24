using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mgh", "notify-migration-task-state")]
public record AwsMghNotifyMigrationTaskStateOptions(
[property: CommandSwitch("--progress-update-stream")] string ProgressUpdateStream,
[property: CommandSwitch("--migration-task-name")] string MigrationTaskName,
[property: CommandSwitch("--task")] string Task,
[property: CommandSwitch("--update-date-time")] long UpdateDateTime,
[property: CommandSwitch("--next-update-seconds")] int NextUpdateSeconds
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}