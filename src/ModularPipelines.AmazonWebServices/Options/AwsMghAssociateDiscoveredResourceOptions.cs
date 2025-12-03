using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mgh", "associate-discovered-resource")]
public record AwsMghAssociateDiscoveredResourceOptions(
[property: CliOption("--progress-update-stream")] string ProgressUpdateStream,
[property: CliOption("--migration-task-name")] string MigrationTaskName,
[property: CliOption("--discovered-resource")] string DiscoveredResource
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}