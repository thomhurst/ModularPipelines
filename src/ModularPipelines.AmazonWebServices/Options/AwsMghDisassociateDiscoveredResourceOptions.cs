using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mgh", "disassociate-discovered-resource")]
public record AwsMghDisassociateDiscoveredResourceOptions(
[property: CliOption("--progress-update-stream")] string ProgressUpdateStream,
[property: CliOption("--migration-task-name")] string MigrationTaskName,
[property: CliOption("--configuration-id")] string ConfigurationId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}