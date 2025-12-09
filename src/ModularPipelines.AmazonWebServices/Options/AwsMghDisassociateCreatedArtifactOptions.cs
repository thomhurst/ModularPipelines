using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mgh", "disassociate-created-artifact")]
public record AwsMghDisassociateCreatedArtifactOptions(
[property: CliOption("--progress-update-stream")] string ProgressUpdateStream,
[property: CliOption("--migration-task-name")] string MigrationTaskName,
[property: CliOption("--created-artifact-name")] string CreatedArtifactName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}