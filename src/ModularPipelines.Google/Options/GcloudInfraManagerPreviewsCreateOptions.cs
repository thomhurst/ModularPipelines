using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("infra-manager", "previews", "create")]
public record GcloudInfraManagerPreviewsCreateOptions(
[property: PositionalArgument] string Preview
) : GcloudOptions
{
    [CommandSwitch("--artifacts-gcs-bucket")]
    public string? ArtifactsGcsBucket { get; set; }

    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--deployment")]
    public string? Deployment { get; set; }

    [CommandSwitch("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--preview-mode")]
    public string? PreviewMode { get; set; }

    [CommandSwitch("--service-account")]
    public string? ServiceAccount { get; set; }

    [CommandSwitch("--worker-pool")]
    public string? WorkerPool { get; set; }

    [CommandSwitch("--gcs-source")]
    public string? GcsSource { get; set; }

    [CommandSwitch("--git-source-directory")]
    public string? GitSourceDirectory { get; set; }

    [CommandSwitch("--git-source-ref")]
    public string? GitSourceRef { get; set; }

    [CommandSwitch("--git-source-repo")]
    public string? GitSourceRepo { get; set; }

    [CommandSwitch("--ignore-file")]
    public string? IgnoreFile { get; set; }

    [CommandSwitch("--local-source")]
    public string? LocalSource { get; set; }

    [CommandSwitch("--input-values")]
    public IEnumerable<KeyValue>? InputValues { get; set; }

    [CommandSwitch("--inputs-file")]
    public string? InputsFile { get; set; }
}