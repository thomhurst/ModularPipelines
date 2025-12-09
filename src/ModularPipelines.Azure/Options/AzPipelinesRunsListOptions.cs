using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("pipelines", "runs", "list")]
public record AzPipelinesRunsListOptions : AzOptions
{
    [CliOption("--branch")]
    public string? Branch { get; set; }

    [CliFlag("--detect")]
    public bool? Detect { get; set; }

    [CliOption("--org")]
    public string? Org { get; set; }

    [CliOption("--pipeline-ids")]
    public string? PipelineIds { get; set; }

    [CliOption("--project")]
    public string? Project { get; set; }

    [CliOption("--query-order")]
    public string? QueryOrder { get; set; }

    [CliOption("--reason")]
    public string? Reason { get; set; }

    [CliOption("--requested-for")]
    public string? RequestedFor { get; set; }

    [CliOption("--result")]
    public string? Result { get; set; }

    [CliOption("--status")]
    public string? Status { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--top")]
    public string? Top { get; set; }
}