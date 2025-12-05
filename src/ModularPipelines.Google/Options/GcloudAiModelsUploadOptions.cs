using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ai", "models", "upload")]
public record GcloudAiModelsUploadOptions(
[property: CliOption("--container-image-uri")] string ContainerImageUri,
[property: CliOption("--display-name")] string DisplayName
) : GcloudOptions
{
    [CliOption("--artifact-uri")]
    public string? ArtifactUri { get; set; }

    [CliOption("--container-args")]
    public string[]? ContainerArgs { get; set; }

    [CliOption("--container-command")]
    public string[]? ContainerCommand { get; set; }

    [CliOption("--container-deployment-timeout-seconds")]
    public string? ContainerDeploymentTimeoutSeconds { get; set; }

    [CliOption("--container-env-vars")]
    public IEnumerable<KeyValue>? ContainerEnvVars { get; set; }

    [CliOption("--container-grpc-ports")]
    public string[]? ContainerGrpcPorts { get; set; }

    [CliOption("--container-health-probe-exec")]
    public string[]? ContainerHealthProbeExec { get; set; }

    [CliOption("--container-health-probe-period-seconds")]
    public string? ContainerHealthProbePeriodSeconds { get; set; }

    [CliOption("--container-health-probe-timeout-seconds")]
    public string? ContainerHealthProbeTimeoutSeconds { get; set; }

    [CliOption("--container-health-route")]
    public string? ContainerHealthRoute { get; set; }

    [CliOption("--container-ports")]
    public string[]? ContainerPorts { get; set; }

    [CliOption("--container-predict-route")]
    public string? ContainerPredictRoute { get; set; }

    [CliOption("--container-shared-memory-size-mb")]
    public string? ContainerSharedMemorySizeMb { get; set; }

    [CliOption("--container-startup-probe-exec")]
    public string[]? ContainerStartupProbeExec { get; set; }

    [CliOption("--container-startup-probe-period-seconds")]
    public string? ContainerStartupProbePeriodSeconds { get; set; }

    [CliOption("--container-startup-probe-timeout-seconds")]
    public string? ContainerStartupProbeTimeoutSeconds { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--explanation-metadata-file")]
    public string? ExplanationMetadataFile { get; set; }

    [CliOption("--explanation-method")]
    public string? ExplanationMethod { get; set; }

    [CliOption("--explanation-path-count")]
    public string? ExplanationPathCount { get; set; }

    [CliOption("--explanation-step-count")]
    public string? ExplanationStepCount { get; set; }

    [CliOption("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CliOption("--model-id")]
    public string? ModelId { get; set; }

    [CliOption("--parent-model")]
    public string? ParentModel { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }

    [CliOption("--smooth-grad-noise-sigma")]
    public string? SmoothGradNoiseSigma { get; set; }

    [CliOption("--smooth-grad-noise-sigma-by-feature")]
    public IEnumerable<KeyValue>? SmoothGradNoiseSigmaByFeature { get; set; }

    [CliOption("--smooth-grad-noisy-sample-count")]
    public string? SmoothGradNoisySampleCount { get; set; }

    [CliOption("--version-aliases")]
    public string[]? VersionAliases { get; set; }

    [CliOption("--version-description")]
    public string? VersionDescription { get; set; }
}