using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mgh", "disassociate-created-artifact")]
public record AwsMghDisassociateCreatedArtifactOptions(
[property: CommandSwitch("--progress-update-stream")] string ProgressUpdateStream,
[property: CommandSwitch("--migration-task-name")] string MigrationTaskName,
[property: CommandSwitch("--created-artifact-name")] string CreatedArtifactName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}