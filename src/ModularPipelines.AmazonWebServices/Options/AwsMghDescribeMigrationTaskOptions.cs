using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mgh", "describe-migration-task")]
public record AwsMghDescribeMigrationTaskOptions(
[property: CommandSwitch("--progress-update-stream")] string ProgressUpdateStream,
[property: CommandSwitch("--migration-task-name")] string MigrationTaskName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}