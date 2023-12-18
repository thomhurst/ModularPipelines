using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("containerapp", "create")]
public record AzContainerappCreateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [BooleanCommandSwitch("--allow-insecure")]
    public bool? AllowInsecure { get; set; }

    [CommandSwitch("--args")]
    public string? Args { get; set; }

    [CommandSwitch("--artifact")]
    public string? Artifact { get; set; }

    [CommandSwitch("--bind")]
    public string? Bind { get; set; }

    [CommandSwitch("--branch")]
    public string? Branch { get; set; }

    [CommandSwitch("--command")]
    public string? Command { get; set; }

    [CommandSwitch("--container-name")]
    public string? ContainerName { get; set; }

    [CommandSwitch("--context-path")]
    public string? ContextPath { get; set; }

    [CommandSwitch("--cpu")]
    public string? Cpu { get; set; }

    [CommandSwitch("--customized-keys")]
    public string? CustomizedKeys { get; set; }

    [BooleanCommandSwitch("--dal")]
    public bool? Dal { get; set; }

    [CommandSwitch("--dapr-app-id")]
    public string? DaprAppId { get; set; }

    [CommandSwitch("--dapr-app-port")]
    public string? DaprAppPort { get; set; }

    [CommandSwitch("--dapr-app-protocol")]
    public string? DaprAppProtocol { get; set; }

    [CommandSwitch("--dapr-http-max-request-size")]
    public string? DaprHttpMaxRequestSize { get; set; }

    [CommandSwitch("--dapr-http-read-buffer-size")]
    public string? DaprHttpReadBufferSize { get; set; }

    [CommandSwitch("--dapr-log-level")]
    public string? DaprLogLevel { get; set; }

    [BooleanCommandSwitch("--enable-dapr")]
    public bool? EnableDapr { get; set; }

    [CommandSwitch("--env-vars")]
    public string? EnvVars { get; set; }

    [CommandSwitch("--environment")]
    public string? Environment { get; set; }

    [CommandSwitch("--environment-type")]
    public string? EnvironmentType { get; set; }

    [CommandSwitch("--exposed-port")]
    public string? ExposedPort { get; set; }

    [CommandSwitch("--image")]
    public string? Image { get; set; }

    [CommandSwitch("--ingress")]
    public string? Ingress { get; set; }

    [CommandSwitch("--max-replicas")]
    public string? MaxReplicas { get; set; }

    [CommandSwitch("--memory")]
    public string? Memory { get; set; }

    [CommandSwitch("--min-replicas")]
    public string? MinReplicas { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--registry-identity")]
    public string? RegistryIdentity { get; set; }

    [CommandSwitch("--registry-password")]
    public string? RegistryPassword { get; set; }

    [CommandSwitch("--registry-server")]
    public string? RegistryServer { get; set; }

    [CommandSwitch("--registry-username")]
    public string? RegistryUsername { get; set; }

    [CommandSwitch("--repo")]
    public string? Repo { get; set; }

    [CommandSwitch("--revision-suffix")]
    public string? RevisionSuffix { get; set; }

    [CommandSwitch("--revisions-mode")]
    public string? RevisionsMode { get; set; }

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

    [CommandSwitch("--secret-volume-mount")]
    public string? SecretVolumeMount { get; set; }

    [CommandSwitch("--secrets")]
    public string? Secrets { get; set; }

    [CommandSwitch("--service-principal-client-id")]
    public string? ServicePrincipalClientId { get; set; }

    [CommandSwitch("--service-principal-client-secret")]
    public string? ServicePrincipalClientSecret { get; set; }

    [CommandSwitch("--service-principal-tenant-id")]
    public string? ServicePrincipalTenantId { get; set; }

    [CommandSwitch("--source")]
    public string? Source { get; set; }

    [BooleanCommandSwitch("--system-assigned")]
    public bool? SystemAssigned { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--target-port")]
    public string? TargetPort { get; set; }

    [CommandSwitch("--termination-grace-period")]
    public string? TerminationGracePeriod { get; set; }

    [CommandSwitch("--token")]
    public string? Token { get; set; }

    [CommandSwitch("--transport")]
    public string? Transport { get; set; }

    [CommandSwitch("--user-assigned")]
    public string? UserAssigned { get; set; }

    [CommandSwitch("--workload-profile-name")]
    public string? WorkloadProfileName { get; set; }

    [CommandSwitch("--yaml")]
    public string? Yaml { get; set; }
}

