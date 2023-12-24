using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mgh", "associate-discovered-resource")]
public record AwsMghAssociateDiscoveredResourceOptions(
[property: CommandSwitch("--progress-update-stream")] string ProgressUpdateStream,
[property: CommandSwitch("--migration-task-name")] string MigrationTaskName,
[property: CommandSwitch("--discovered-resource")] string DiscoveredResource
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}