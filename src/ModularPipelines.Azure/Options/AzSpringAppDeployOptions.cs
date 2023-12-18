using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("spring", "app", "deploy")]
public record AzSpringAppDeployOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--service")] string Service
) : AzOptions
{
    [CommandSwitch("--apms")]
    public string? Apms { get; set; }

    [CommandSwitch("--artifact-path")]
    public string? ArtifactPath { get; set; }

    [CommandSwitch("--build-certificates")]
    public string? BuildCertificates { get; set; }

    [CommandSwitch("--build-cpu")]
    public string? BuildCpu { get; set; }

    [CommandSwitch("--build-env")]
    public string? BuildEnv { get; set; }

    [CommandSwitch("--build-memory")]
    public string? BuildMemory { get; set; }

    [CommandSwitch("--builder")]
    public string? Builder { get; set; }

    [CommandSwitch("--config-file-patterns")]
    public string? ConfigFilePatterns { get; set; }

    [CommandSwitch("--container-args")]
    public string? ContainerArgs { get; set; }

    [CommandSwitch("--container-command")]
    public string? ContainerCommand { get; set; }

    [CommandSwitch("--container-image")]
    public string? ContainerImage { get; set; }

    [CommandSwitch("--container-registry")]
    public string? ContainerRegistry { get; set; }

    [CommandSwitch("--deployment")]
    public string? Deployment { get; set; }

    [BooleanCommandSwitch("--disable-app-log")]
    public bool? DisableAppLog { get; set; }

    [BooleanCommandSwitch("--disable-probe")]
    public bool? DisableProbe { get; set; }

    [BooleanCommandSwitch("--disable-validation")]
    public bool? DisableValidation { get; set; }

    [BooleanCommandSwitch("--enable-liveness-probe")]
    public bool? EnableLivenessProbe { get; set; }

    [BooleanCommandSwitch("--enable-readiness-probe")]
    public bool? EnableReadinessProbe { get; set; }

    [BooleanCommandSwitch("--enable-startup-probe")]
    public bool? EnableStartupProbe { get; set; }

    [CommandSwitch("--env")]
    public string? Env { get; set; }

    [CommandSwitch("--grace-period")]
    public string? GracePeriod { get; set; }

    [CommandSwitch("--jvm-options")]
    public string? JvmOptions { get; set; }

    [CommandSwitch("--language-framework")]
    public string? LanguageFramework { get; set; }

    [CommandSwitch("--liveness-probe-config")]
    public string? LivenessProbeConfig { get; set; }

    [CommandSwitch("--main-entry")]
    public string? MainEntry { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--readiness-probe-config")]
    public string? ReadinessProbeConfig { get; set; }

    [CommandSwitch("--registry-password")]
    public string? RegistryPassword { get; set; }

    [CommandSwitch("--registry-username")]
    public string? RegistryUsername { get; set; }

    [CommandSwitch("--runtime-version")]
    public string? RuntimeVersion { get; set; }

    [CommandSwitch("--server-version")]
    public string? ServerVersion { get; set; }

    [CommandSwitch("--source-path")]
    public string? SourcePath { get; set; }

    [CommandSwitch("--startup-probe-config")]
    public string? StartupProbeConfig { get; set; }

    [CommandSwitch("--target-module")]
    public string? TargetModule { get; set; }

    [CommandSwitch("--version")]
    public string? Version { get; set; }
}