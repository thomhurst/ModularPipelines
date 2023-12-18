using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pipelines", "runs", "list")]
public record AzPipelinesRunsListOptions : AzOptions
{
    [CommandSwitch("--branch")]
    public string? Branch { get; set; }

    [BooleanCommandSwitch("--detect")]
    public bool? Detect { get; set; }

    [CommandSwitch("--org")]
    public string? Org { get; set; }

    [CommandSwitch("--pipeline-ids")]
    public string? PipelineIds { get; set; }

    [CommandSwitch("--project")]
    public string? Project { get; set; }

    [CommandSwitch("--query-order")]
    public string? QueryOrder { get; set; }

    [CommandSwitch("--reason")]
    public string? Reason { get; set; }

    [CommandSwitch("--requested-for")]
    public string? RequestedFor { get; set; }

    [CommandSwitch("--result")]
    public string? Result { get; set; }

    [CommandSwitch("--status")]
    public string? Status { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--top")]
    public string? Top { get; set; }
}