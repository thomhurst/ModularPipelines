using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("pipelines", "variable", "create")]
public record AzPipelinesVariableCreateOptions(
[property: CliOption("--name")] string Name
) : AzOptions
{
    [CliFlag("--allow-override")]
    public bool? AllowOverride { get; set; }

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

    [CliFlag("--secret")]
    public bool? Secret { get; set; }

    [CliOption("--value")]
    public string? Value { get; set; }
}