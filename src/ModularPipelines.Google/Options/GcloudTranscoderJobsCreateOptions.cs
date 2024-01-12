using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("transcoder", "jobs", "create")]
public record GcloudTranscoderJobsCreateOptions : GcloudOptions
{
    [CommandSwitch("--batch-mode-priority")]
    public string? BatchModePriority { get; set; }

    [CommandSwitch("--input-uri")]
    public string? InputUri { get; set; }

    [CommandSwitch("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--mode")]
    public string? Mode { get; set; }

    [CommandSwitch("--optimization")]
    public string? Optimization { get; set; }

    [CommandSwitch("--output-uri")]
    public string? OutputUri { get; set; }

    [CommandSwitch("--file")]
    public string? File { get; set; }

    [CommandSwitch("--json")]
    public string? Json { get; set; }

    [CommandSwitch("--template-id")]
    public string? TemplateId { get; set; }
}