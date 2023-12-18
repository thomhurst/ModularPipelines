using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("spring", "app", "update")]
public record AzSpringAppUpdateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--service")] string Service
) : AzOptions
{
    [BooleanCommandSwitch("--assign-endpoint")]
    public bool? AssignEndpoint { get; set; }

    [BooleanCommandSwitch("--assign-public-endpoint")]
    public bool? AssignPublicEndpoint { get; set; }

    [CommandSwitch("--backend-protocol")]
    public string? BackendProtocol { get; set; }

    [CommandSwitch("--client-auth-certs")]
    public string? ClientAuthCerts { get; set; }

    [CommandSwitch("--config-file-patterns")]
    public string? ConfigFilePatterns { get; set; }

    [CommandSwitch("--deployment")]
    public string? Deployment { get; set; }

    [BooleanCommandSwitch("--disable-probe")]
    public bool? DisableProbe { get; set; }

    [BooleanCommandSwitch("--enable-ingress-to-app-tls")]
    public bool? EnableIngressToAppTls { get; set; }

    [BooleanCommandSwitch("--enable-liveness-probe")]
    public bool? EnableLivenessProbe { get; set; }

    [BooleanCommandSwitch("--enable-persistent-storage")]
    public bool? EnablePersistentStorage { get; set; }

    [BooleanCommandSwitch("--enable-readiness-probe")]
    public bool? EnableReadinessProbe { get; set; }

    [BooleanCommandSwitch("--enable-startup-probe")]
    public bool? EnableStartupProbe { get; set; }

    [CommandSwitch("--env")]
    public string? Env { get; set; }

    [CommandSwitch("--grace-period")]
    public string? GracePeriod { get; set; }

    [BooleanCommandSwitch("--https-only")]
    public bool? HttpsOnly { get; set; }

    [CommandSwitch("--ingress-read-timeout")]
    public string? IngressReadTimeout { get; set; }

    [CommandSwitch("--ingress-send-timeout")]
    public string? IngressSendTimeout { get; set; }

    [CommandSwitch("--jvm-options")]
    public string? JvmOptions { get; set; }

    [CommandSwitch("--liveness-probe-config")]
    public string? LivenessProbeConfig { get; set; }

    [CommandSwitch("--loaded-public-certificate-file")]
    public string? LoadedPublicCertificateFile { get; set; }

    [CommandSwitch("--main-entry")]
    public string? MainEntry { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--persistent-storage")]
    public string? PersistentStorage { get; set; }

    [CommandSwitch("--readiness-probe-config")]
    public string? ReadinessProbeConfig { get; set; }

    [CommandSwitch("--runtime-version")]
    public string? RuntimeVersion { get; set; }

    [CommandSwitch("--secrets")]
    public string? Secrets { get; set; }

    [CommandSwitch("--session-affinity")]
    public string? SessionAffinity { get; set; }

    [CommandSwitch("--session-max-age")]
    public string? SessionMaxAge { get; set; }

    [CommandSwitch("--startup-probe-config")]
    public string? StartupProbeConfig { get; set; }

    [CommandSwitch("--workload-profile")]
    public string? WorkloadProfile { get; set; }
}