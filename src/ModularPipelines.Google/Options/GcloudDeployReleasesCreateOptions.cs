using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("deploy", "releases", "create")]
public record GcloudDeployReleasesCreateOptions(
[property: CliArgument] string Release,
[property: CliArgument] string DeliveryPipeline,
[property: CliArgument] string Region
) : GcloudOptions
{
    [CliOption("--annotations")]
    public IEnumerable<KeyValue>? Annotations { get; set; }

    [CliOption("--deploy-parameters")]
    public IEnumerable<KeyValue>? DeployParameters { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--gcs-source-staging-dir")]
    public string? GcsSourceStagingDir { get; set; }

    [CliOption("--ignore-file")]
    public string? IgnoreFile { get; set; }

    [CliOption("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CliOption("--skaffold-version")]
    public string? SkaffoldVersion { get; set; }

    [CliOption("--to-target")]
    public string? ToTarget { get; set; }

    [CliOption("--build-artifacts")]
    public string? BuildArtifacts { get; set; }

    [CliOption("--images")]
    public string[]? Images { get; set; }

    [CliFlag("--disable-initial-rollout")]
    public bool? DisableInitialRollout { get; set; }

    [CliFlag("--enable-initial-rollout")]
    public bool? EnableInitialRollout { get; set; }

    [CliOption("--initial-rollout-annotations")]
    public IEnumerable<KeyValue>? InitialRolloutAnnotations { get; set; }

    [CliOption("--initial-rollout-labels")]
    public IEnumerable<KeyValue>? InitialRolloutLabels { get; set; }

    [CliOption("--initial-rollout-phase-id")]
    public string? InitialRolloutPhaseId { get; set; }

    [CliOption("--from-k8s-manifest")]
    public string? FromK8sManifest { get; set; }

    [CliOption("--from-run-manifest")]
    public string? FromRunManifest { get; set; }

    [CliOption("--skaffold-file")]
    public string? SkaffoldFile { get; set; }

    [CliOption("--source")]
    public string? Source { get; set; }
}