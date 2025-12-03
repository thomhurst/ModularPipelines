using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("acr", "pipeline-run", "create")]
public record AzAcrPipelineRunCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--pipeline")] string Pipeline,
[property: CliOption("--pipeline-type")] string PipelineType,
[property: CliOption("--registry")] string Registry,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--storage-blob")] string StorageBlob
) : AzOptions
{
    [CliOption("--artifacts")]
    public string? Artifacts { get; set; }

    [CliFlag("--force-redeploy")]
    public bool? ForceRedeploy { get; set; }
}