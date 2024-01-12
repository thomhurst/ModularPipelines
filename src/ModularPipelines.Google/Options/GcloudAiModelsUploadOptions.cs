using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ai", "models", "upload")]
public record GcloudAiModelsUploadOptions(
[property: CommandSwitch("--container-image-uri")] string ContainerImageUri,
[property: CommandSwitch("--display-name")] string DisplayName
) : GcloudOptions
{
    [CommandSwitch("--artifact-uri")]
    public string? ArtifactUri { get; set; }

    [CommandSwitch("--container-args")]
    public string[]? ContainerArgs { get; set; }

    [CommandSwitch("--container-command")]
    public string[]? ContainerCommand { get; set; }

    [CommandSwitch("--container-deployment-timeout-seconds")]
    public string? ContainerDeploymentTimeoutSeconds { get; set; }

    [CommandSwitch("--container-env-vars")]
    public IEnumerable<KeyValue>? ContainerEnvVars { get; set; }

    [CommandSwitch("--container-grpc-ports")]
    public string[]? ContainerGrpcPorts { get; set; }

    [CommandSwitch("--container-health-probe-exec")]
    public string[]? ContainerHealthProbeExec { get; set; }

    [CommandSwitch("--container-health-probe-period-seconds")]
    public string? ContainerHealthProbePeriodSeconds { get; set; }

    [CommandSwitch("--container-health-probe-timeout-seconds")]
    public string? ContainerHealthProbeTimeoutSeconds { get; set; }

    [CommandSwitch("--container-health-route")]
    public string? ContainerHealthRoute { get; set; }

    [CommandSwitch("--container-ports")]
    public string[]? ContainerPorts { get; set; }

    [CommandSwitch("--container-predict-route")]
    public string? ContainerPredictRoute { get; set; }

    [CommandSwitch("--container-shared-memory-size-mb")]
    public string? ContainerSharedMemorySizeMb { get; set; }

    [CommandSwitch("--container-startup-probe-exec")]
    public string[]? ContainerStartupProbeExec { get; set; }

    [CommandSwitch("--container-startup-probe-period-seconds")]
    public string? ContainerStartupProbePeriodSeconds { get; set; }

    [CommandSwitch("--container-startup-probe-timeout-seconds")]
    public string? ContainerStartupProbeTimeoutSeconds { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--explanation-metadata-file")]
    public string? ExplanationMetadataFile { get; set; }

    [CommandSwitch("--explanation-method")]
    public string? ExplanationMethod { get; set; }

    [CommandSwitch("--explanation-path-count")]
    public string? ExplanationPathCount { get; set; }

    [CommandSwitch("--explanation-step-count")]
    public string? ExplanationStepCount { get; set; }

    [CommandSwitch("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CommandSwitch("--model-id")]
    public string? ModelId { get; set; }

    [CommandSwitch("--parent-model")]
    public string? ParentModel { get; set; }

    [CommandSwitch("--region")]
    public string? Region { get; set; }

    [CommandSwitch("--smooth-grad-noise-sigma")]
    public string? SmoothGradNoiseSigma { get; set; }

    [CommandSwitch("--smooth-grad-noise-sigma-by-feature")]
    public IEnumerable<KeyValue>? SmoothGradNoiseSigmaByFeature { get; set; }

    [CommandSwitch("--smooth-grad-noisy-sample-count")]
    public string? SmoothGradNoisySampleCount { get; set; }

    [CommandSwitch("--version-aliases")]
    public string[]? VersionAliases { get; set; }

    [CommandSwitch("--version-description")]
    public string? VersionDescription { get; set; }
}