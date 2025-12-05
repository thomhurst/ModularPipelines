using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("transcoder", "jobs", "create")]
public record GcloudTranscoderJobsCreateOptions : GcloudOptions
{
    [CliOption("--batch-mode-priority")]
    public string? BatchModePriority { get; set; }

    [CliOption("--input-uri")]
    public string? InputUri { get; set; }

    [CliOption("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--mode")]
    public string? Mode { get; set; }

    [CliOption("--optimization")]
    public string? Optimization { get; set; }

    [CliOption("--output-uri")]
    public string? OutputUri { get; set; }

    [CliOption("--file")]
    public string? File { get; set; }

    [CliOption("--json")]
    public string? Json { get; set; }

    [CliOption("--template-id")]
    public string? TemplateId { get; set; }
}