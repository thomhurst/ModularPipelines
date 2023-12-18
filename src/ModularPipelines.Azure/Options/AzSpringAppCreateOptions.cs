using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("spring", "app", "create")]
public record AzSpringAppCreateOptions(
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

    [CommandSwitch("--cpu")]
    public string? Cpu { get; set; }

    [CommandSwitch("--deployment-name")]
    public string? DeploymentName { get; set; }

    [BooleanCommandSwitch("--disable-probe")]
    public bool? DisableProbe { get; set; }

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

    [CommandSwitch("--ingress-read-timeout")]
    public string? IngressReadTimeout { get; set; }

    [CommandSwitch("--ingress-send-timeout")]
    public string? IngressSendTimeout { get; set; }

    [CommandSwitch("--instance-count")]
    public int? InstanceCount { get; set; }

    [CommandSwitch("--jvm-options")]
    public string? JvmOptions { get; set; }

    [CommandSwitch("--liveness-probe-config")]
    public string? LivenessProbeConfig { get; set; }

    [CommandSwitch("--loaded-public-certificate-file")]
    public string? LoadedPublicCertificateFile { get; set; }

    [CommandSwitch("--max-replicas")]
    public string? MaxReplicas { get; set; }

    [CommandSwitch("--memory")]
    public string? Memory { get; set; }

    [CommandSwitch("--min-replicas")]
    public string? MinReplicas { get; set; }

    [CommandSwitch("--persistent-storage")]
    public string? PersistentStorage { get; set; }

    [CommandSwitch("--readiness-probe-config")]
    public string? ReadinessProbeConfig { get; set; }

    [CommandSwitch("--runtime-version")]
    public string? RuntimeVersion { get; set; }

    [CommandSwitch("--scale-rule-auth")]
    public string? ScaleRuleAuth { get; set; }

    [CommandSwitch("--scale-rule-http-concurrency")]
    public string? ScaleRuleHttpConcurrency { get; set; }

    [CommandSwitch("--scale-rule-metadata")]
    public string? ScaleRuleMetadata { get; set; }

    [CommandSwitch("--scale-rule-name")]
    public string? ScaleRuleName { get; set; }

    [CommandSwitch("--scale-rule-type")]
    public string? ScaleRuleType { get; set; }

    [CommandSwitch("--secrets")]
    public string? Secrets { get; set; }

    [CommandSwitch("--session-affinity")]
    public string? SessionAffinity { get; set; }

    [CommandSwitch("--session-max-age")]
    public string? SessionMaxAge { get; set; }

    [CommandSwitch("--startup-probe-config")]
    public string? StartupProbeConfig { get; set; }

    [BooleanCommandSwitch("--system-assigned")]
    public bool? SystemAssigned { get; set; }

    [CommandSwitch("--user-assigned")]
    public string? UserAssigned { get; set; }

    [CommandSwitch("--workload-profile")]
    public string? WorkloadProfile { get; set; }
}