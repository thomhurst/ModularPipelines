using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mgh", "associate-created-artifact")]
public record AwsMghAssociateCreatedArtifactOptions(
[property: CommandSwitch("--progress-update-stream")] string ProgressUpdateStream,
[property: CommandSwitch("--migration-task-name")] string MigrationTaskName,
[property: CommandSwitch("--created-artifact")] string CreatedArtifact
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}