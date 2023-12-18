using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("acr", "pipeline-run", "create")]
public record AzAcrPipelineRunCreateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--pipeline")] string Pipeline,
[property: CommandSwitch("--pipeline-type")] string PipelineType,
[property: CommandSwitch("--registry")] string Registry,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--storage-blob")] string StorageBlob
) : AzOptions
{
    [CommandSwitch("--artifacts")]
    public string? Artifacts { get; set; }

    [BooleanCommandSwitch("--force-redeploy")]
    public bool? ForceRedeploy { get; set; }
}