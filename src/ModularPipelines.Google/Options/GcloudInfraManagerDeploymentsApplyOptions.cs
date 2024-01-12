using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("infra-manager", "deployments", "apply")]
public record GcloudInfraManagerDeploymentsApplyOptions(
[property: PositionalArgument] string Deployment,
[property: PositionalArgument] string Location
) : GcloudOptions
{
    [CommandSwitch("--artifacts-gcs-bucket")]
    public string? ArtifactsGcsBucket { get; set; }

    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [BooleanCommandSwitch("--import-existing-resources")]
    public bool? ImportExistingResources { get; set; }

    [CommandSwitch("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

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