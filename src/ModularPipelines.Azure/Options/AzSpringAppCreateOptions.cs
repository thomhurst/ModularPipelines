using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("spring", "app", "create")]
public record AzSpringAppCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--service")] string Service
) : AzOptions
{
    [CliFlag("--assign-endpoint")]
    public bool? AssignEndpoint { get; set; }

    [CliFlag("--assign-public-endpoint")]
    public bool? AssignPublicEndpoint { get; set; }

    [CliOption("--backend-protocol")]
    public string? BackendProtocol { get; set; }

    [CliOption("--client-auth-certs")]
    public string? ClientAuthCerts { get; set; }

    [CliOption("--cpu")]
    public string? Cpu { get; set; }

    [CliOption("--deployment-name")]
    public string? DeploymentName { get; set; }

    [CliFlag("--disable-probe")]
    public bool? DisableProbe { get; set; }

    [CliFlag("--enable-liveness-probe")]
    public bool? EnableLivenessProbe { get; set; }

    [CliFlag("--enable-persistent-storage")]
    public bool? EnablePersistentStorage { get; set; }

    [CliFlag("--enable-readiness-probe")]
    public bool? EnableReadinessProbe { get; set; }

    [CliFlag("--enable-startup-probe")]
    public bool? EnableStartupProbe { get; set; }

    [CliOption("--env")]
    public string? Env { get; set; }

    [CliOption("--grace-period")]
    public string? GracePeriod { get; set; }

    [CliOption("--ingress-read-timeout")]
    public string? IngressReadTimeout { get; set; }

    [CliOption("--ingress-send-timeout")]
    public string? IngressSendTimeout { get; set; }

    [CliOption("--instance-count")]
    public int? InstanceCount { get; set; }

    [CliOption("--jvm-options")]
    public string? JvmOptions { get; set; }

    [CliOption("--liveness-probe-config")]
    public string? LivenessProbeConfig { get; set; }

    [CliOption("--loaded-public-certificate-file")]
    public string? LoadedPublicCertificateFile { get; set; }

    [CliOption("--max-replicas")]
    public string? MaxReplicas { get; set; }

    [CliOption("--memory")]
    public string? Memory { get; set; }

    [CliOption("--min-replicas")]
    public string? MinReplicas { get; set; }

    [CliOption("--persistent-storage")]
    public string? PersistentStorage { get; set; }

    [CliOption("--readiness-probe-config")]
    public string? ReadinessProbeConfig { get; set; }

    [CliOption("--runtime-version")]
    public string? RuntimeVersion { get; set; }

    [CliOption("--scale-rule-auth")]
    public string? ScaleRuleAuth { get; set; }

    [CliOption("--scale-rule-http-concurrency")]
    public string? ScaleRuleHttpConcurrency { get; set; }

    [CliOption("--scale-rule-metadata")]
    public string? ScaleRuleMetadata { get; set; }

    [CliOption("--scale-rule-name")]
    public string? ScaleRuleName { get; set; }

    [CliOption("--scale-rule-type")]
    public string? ScaleRuleType { get; set; }

    [CliOption("--secrets")]
    public string? Secrets { get; set; }

    [CliOption("--session-affinity")]
    public string? SessionAffinity { get; set; }

    [CliOption("--session-max-age")]
    public string? SessionMaxAge { get; set; }

    [CliOption("--startup-probe-config")]
    public string? StartupProbeConfig { get; set; }

    [CliFlag("--system-assigned")]
    public bool? SystemAssigned { get; set; }

    [CliOption("--user-assigned")]
    public string? UserAssigned { get; set; }

    [CliOption("--workload-profile")]
    public string? WorkloadProfile { get; set; }
}