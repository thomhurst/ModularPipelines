using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("infra-manager", "previews", "create")]
public record GcloudInfraManagerPreviewsCreateOptions(
[property: CliArgument] string Preview
) : GcloudOptions
{
    [CliOption("--artifacts-gcs-bucket")]
    public string? ArtifactsGcsBucket { get; set; }

    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--deployment")]
    public string? Deployment { get; set; }

    [CliOption("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--preview-mode")]
    public string? PreviewMode { get; set; }

    [CliOption("--service-account")]
    public string? ServiceAccount { get; set; }

    [CliOption("--worker-pool")]
    public string? WorkerPool { get; set; }

    [CliOption("--gcs-source")]
    public string? GcsSource { get; set; }

    [CliOption("--git-source-directory")]
    public string? GitSourceDirectory { get; set; }

    [CliOption("--git-source-ref")]
    public string? GitSourceRef { get; set; }

    [CliOption("--git-source-repo")]
    public string? GitSourceRepo { get; set; }

    [CliOption("--ignore-file")]
    public string? IgnoreFile { get; set; }

    [CliOption("--local-source")]
    public string? LocalSource { get; set; }

    [CliOption("--input-values")]
    public IEnumerable<KeyValue>? InputValues { get; set; }

    [CliOption("--inputs-file")]
    public string? InputsFile { get; set; }
}