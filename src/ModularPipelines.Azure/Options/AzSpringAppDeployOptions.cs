using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("spring", "app", "deploy")]
public record AzSpringAppDeployOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--service")] string Service
) : AzOptions
{
    [CliOption("--apms")]
    public string? Apms { get; set; }

    [CliOption("--artifact-path")]
    public string? ArtifactPath { get; set; }

    [CliOption("--build-certificates")]
    public string? BuildCertificates { get; set; }

    [CliOption("--build-cpu")]
    public string? BuildCpu { get; set; }

    [CliOption("--build-env")]
    public string? BuildEnv { get; set; }

    [CliOption("--build-memory")]
    public string? BuildMemory { get; set; }

    [CliOption("--builder")]
    public string? Builder { get; set; }

    [CliOption("--config-file-patterns")]
    public string? ConfigFilePatterns { get; set; }

    [CliOption("--container-args")]
    public string? ContainerArgs { get; set; }

    [CliOption("--container-command")]
    public string? ContainerCommand { get; set; }

    [CliOption("--container-image")]
    public string? ContainerImage { get; set; }

    [CliOption("--container-registry")]
    public string? ContainerRegistry { get; set; }

    [CliOption("--deployment")]
    public string? Deployment { get; set; }

    [CliFlag("--disable-app-log")]
    public bool? DisableAppLog { get; set; }

    [CliFlag("--disable-probe")]
    public bool? DisableProbe { get; set; }

    [CliFlag("--disable-validation")]
    public bool? DisableValidation { get; set; }

    [CliFlag("--enable-liveness-probe")]
    public bool? EnableLivenessProbe { get; set; }

    [CliFlag("--enable-readiness-probe")]
    public bool? EnableReadinessProbe { get; set; }

    [CliFlag("--enable-startup-probe")]
    public bool? EnableStartupProbe { get; set; }

    [CliOption("--env")]
    public string? Env { get; set; }

    [CliOption("--grace-period")]
    public string? GracePeriod { get; set; }

    [CliOption("--jvm-options")]
    public string? JvmOptions { get; set; }

    [CliOption("--language-framework")]
    public string? LanguageFramework { get; set; }

    [CliOption("--liveness-probe-config")]
    public string? LivenessProbeConfig { get; set; }

    [CliOption("--main-entry")]
    public string? MainEntry { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--readiness-probe-config")]
    public string? ReadinessProbeConfig { get; set; }

    [CliOption("--registry-password")]
    public string? RegistryPassword { get; set; }

    [CliOption("--registry-username")]
    public string? RegistryUsername { get; set; }

    [CliOption("--runtime-version")]
    public string? RuntimeVersion { get; set; }

    [CliOption("--server-version")]
    public string? ServerVersion { get; set; }

    [CliOption("--source-path")]
    public string? SourcePath { get; set; }

    [CliOption("--startup-probe-config")]
    public string? StartupProbeConfig { get; set; }

    [CliOption("--target-module")]
    public string? TargetModule { get; set; }

    [CliOption("--version")]
    public string? Version { get; set; }
}