using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("deploy", "releases", "create")]
public record GcloudDeployReleasesCreateOptions(
[property: PositionalArgument] string Release,
[property: PositionalArgument] string DeliveryPipeline,
[property: PositionalArgument] string Region
) : GcloudOptions
{
    [CommandSwitch("--annotations")]
    public IEnumerable<KeyValue>? Annotations { get; set; }

    [CommandSwitch("--deploy-parameters")]
    public IEnumerable<KeyValue>? DeployParameters { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--gcs-source-staging-dir")]
    public string? GcsSourceStagingDir { get; set; }

    [CommandSwitch("--ignore-file")]
    public string? IgnoreFile { get; set; }

    [CommandSwitch("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CommandSwitch("--skaffold-version")]
    public string? SkaffoldVersion { get; set; }

    [CommandSwitch("--to-target")]
    public string? ToTarget { get; set; }

    [CommandSwitch("--build-artifacts")]
    public string? BuildArtifacts { get; set; }

    [CommandSwitch("--images")]
    public string[]? Images { get; set; }

    [BooleanCommandSwitch("--disable-initial-rollout")]
    public bool? DisableInitialRollout { get; set; }

    [BooleanCommandSwitch("--enable-initial-rollout")]
    public bool? EnableInitialRollout { get; set; }

    [CommandSwitch("--initial-rollout-annotations")]
    public IEnumerable<KeyValue>? InitialRolloutAnnotations { get; set; }

    [CommandSwitch("--initial-rollout-labels")]
    public IEnumerable<KeyValue>? InitialRolloutLabels { get; set; }

    [CommandSwitch("--initial-rollout-phase-id")]
    public string? InitialRolloutPhaseId { get; set; }

    [CommandSwitch("--from-k8s-manifest")]
    public string? FromK8sManifest { get; set; }

    [CommandSwitch("--from-run-manifest")]
    public string? FromRunManifest { get; set; }

    [CommandSwitch("--skaffold-file")]
    public string? SkaffoldFile { get; set; }

    [CommandSwitch("--source")]
    public string? Source { get; set; }
}