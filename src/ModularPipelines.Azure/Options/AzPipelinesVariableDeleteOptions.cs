using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("pipelines", "variable", "delete")]
public record AzPipelinesVariableDeleteOptions(
[property: CliOption("--name")] string Name
) : AzOptions
{
    [CliFlag("--detect")]
    public bool? Detect { get; set; }

    [CliOption("--org")]
    public string? Org { get; set; }

    [CliOption("--pipeline-id")]
    public string? PipelineId { get; set; }

    [CliOption("--pipeline-name")]
    public string? PipelineName { get; set; }

    [CliOption("--project")]
    public string? Project { get; set; }

    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}