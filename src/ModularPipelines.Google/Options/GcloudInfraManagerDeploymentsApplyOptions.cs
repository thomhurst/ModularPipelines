using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("infra-manager", "deployments", "apply")]
public record GcloudInfraManagerDeploymentsApplyOptions(
[property: CliArgument] string Deployment,
[property: CliArgument] string Location
) : GcloudOptions
{
    [CliOption("--artifacts-gcs-bucket")]
    public string? ArtifactsGcsBucket { get; set; }

    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliFlag("--import-existing-resources")]
    public bool? ImportExistingResources { get; set; }

    [CliOption("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

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