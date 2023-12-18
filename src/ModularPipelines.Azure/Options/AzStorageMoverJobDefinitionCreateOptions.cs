using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storage-mover", "job-definition", "create")]
public record AzStorageMoverJobDefinitionCreateOptions(
[property: CommandSwitch("--copy-mode")] string CopyMode,
[property: CommandSwitch("--job-definition-name")] string JobDefinitionName,
[property: CommandSwitch("--project-name")] string ProjectName,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--source-name")] string SourceName,
[property: CommandSwitch("--storage-mover-name")] string StorageMoverName,
[property: CommandSwitch("--target-name")] string TargetName
) : AzOptions
{
    [CommandSwitch("--agent-name")]
    public string? AgentName { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--source-subpath")]
    public string? SourceSubpath { get; set; }

    [CommandSwitch("--target-subpath")]
    public string? TargetSubpath { get; set; }
}