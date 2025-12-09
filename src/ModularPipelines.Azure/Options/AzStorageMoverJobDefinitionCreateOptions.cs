using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("storage-mover", "job-definition", "create")]
public record AzStorageMoverJobDefinitionCreateOptions(
[property: CliOption("--copy-mode")] string CopyMode,
[property: CliOption("--job-definition-name")] string JobDefinitionName,
[property: CliOption("--project-name")] string ProjectName,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--source-name")] string SourceName,
[property: CliOption("--storage-mover-name")] string StorageMoverName,
[property: CliOption("--target-name")] string TargetName
) : AzOptions
{
    [CliOption("--agent-name")]
    public string? AgentName { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--source-subpath")]
    public string? SourceSubpath { get; set; }

    [CliOption("--target-subpath")]
    public string? TargetSubpath { get; set; }
}