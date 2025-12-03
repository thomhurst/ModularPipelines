using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("containerapp", "create")]
public record AzContainerappCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliFlag("--allow-insecure")]
    public bool? AllowInsecure { get; set; }

    [CliOption("--args")]
    public string? Args { get; set; }

    [CliOption("--artifact")]
    public string? Artifact { get; set; }

    [CliOption("--bind")]
    public string? Bind { get; set; }

    [CliOption("--branch")]
    public string? Branch { get; set; }

    [CliOption("--command")]
    public string? Command { get; set; }

    [CliOption("--container-name")]
    public string? ContainerName { get; set; }

    [CliOption("--context-path")]
    public string? ContextPath { get; set; }

    [CliOption("--cpu")]
    public string? Cpu { get; set; }

    [CliOption("--customized-keys")]
    public string? CustomizedKeys { get; set; }

    [CliFlag("--dal")]
    public bool? Dal { get; set; }

    [CliOption("--dapr-app-id")]
    public string? DaprAppId { get; set; }

    [CliOption("--dapr-app-port")]
    public string? DaprAppPort { get; set; }

    [CliOption("--dapr-app-protocol")]
    public string? DaprAppProtocol { get; set; }

    [CliOption("--dapr-http-max-request-size")]
    public string? DaprHttpMaxRequestSize { get; set; }

    [CliOption("--dapr-http-read-buffer-size")]
    public string? DaprHttpReadBufferSize { get; set; }

    [CliOption("--dapr-log-level")]
    public string? DaprLogLevel { get; set; }

    [CliFlag("--enable-dapr")]
    public bool? EnableDapr { get; set; }

    [CliOption("--env-vars")]
    public string? EnvVars { get; set; }

    [CliOption("--environment")]
    public string? Environment { get; set; }

    [CliOption("--environment-type")]
    public string? EnvironmentType { get; set; }

    [CliOption("--exposed-port")]
    public string? ExposedPort { get; set; }

    [CliOption("--image")]
    public string? Image { get; set; }

    [CliOption("--ingress")]
    public string? Ingress { get; set; }

    [CliOption("--max-replicas")]
    public string? MaxReplicas { get; set; }

    [CliOption("--memory")]
    public string? Memory { get; set; }

    [CliOption("--min-replicas")]
    public string? MinReplicas { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--registry-identity")]
    public string? RegistryIdentity { get; set; }

    [CliOption("--registry-password")]
    public string? RegistryPassword { get; set; }

    [CliOption("--registry-server")]
    public string? RegistryServer { get; set; }

    [CliOption("--registry-username")]
    public string? RegistryUsername { get; set; }

    [CliOption("--repo")]
    public string? Repo { get; set; }

    [CliOption("--revision-suffix")]
    public string? RevisionSuffix { get; set; }

    [CliOption("--revisions-mode")]
    public string? RevisionsMode { get; set; }

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

    [CliOption("--secret-volume-mount")]
    public string? SecretVolumeMount { get; set; }

    [CliOption("--secrets")]
    public string? Secrets { get; set; }

    [CliOption("--service-principal-client-id")]
    public string? ServicePrincipalClientId { get; set; }

    [CliOption("--service-principal-client-secret")]
    public string? ServicePrincipalClientSecret { get; set; }

    [CliOption("--service-principal-tenant-id")]
    public string? ServicePrincipalTenantId { get; set; }

    [CliOption("--source")]
    public string? Source { get; set; }

    [CliFlag("--system-assigned")]
    public bool? SystemAssigned { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--target-port")]
    public string? TargetPort { get; set; }

    [CliOption("--termination-grace-period")]
    public string? TerminationGracePeriod { get; set; }

    [CliOption("--token")]
    public string? Token { get; set; }

    [CliOption("--transport")]
    public string? Transport { get; set; }

    [CliOption("--user-assigned")]
    public string? UserAssigned { get; set; }

    [CliOption("--workload-profile-name")]
    public string? WorkloadProfileName { get; set; }

    [CliOption("--yaml")]
    public string? Yaml { get; set; }
}