using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("containerapp", "show-custom-domain-verification-id")]
public record AzContainerappShowCustomDomainVerificationIdOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions
{
    [CommandSwitch("--artifact")]
    public string? Artifact { get; set; }

    [CommandSwitch("--branch")]
    public string? Branch { get; set; }

    [BooleanCommandSwitch("--browse")]
    public bool? Browse { get; set; }

    [CommandSwitch("--connected-cluster-id")]
    public string? ConnectedClusterId { get; set; }

    [CommandSwitch("--context-path")]
    public string? ContextPath { get; set; }

    [CommandSwitch("--custom-location")]
    public string? CustomLocation { get; set; }

    [CommandSwitch("--env-vars")]
    public string? EnvVars { get; set; }

    [CommandSwitch("--environment")]
    public string? Environment { get; set; }

    [CommandSwitch("--image")]
    public string? Image { get; set; }

    [CommandSwitch("--ingress")]
    public string? Ingress { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--logs-workspace-id")]
    public string? LogsWorkspaceId { get; set; }

    [CommandSwitch("--logs-workspace-key")]
    public string? LogsWorkspaceKey { get; set; }

    [CommandSwitch("--registry-password")]
    public string? RegistryPassword { get; set; }

    [CommandSwitch("--registry-server")]
    public string? RegistryServer { get; set; }

    [CommandSwitch("--registry-username")]
    public string? RegistryUsername { get; set; }

    [CommandSwitch("--repo")]
    public string? Repo { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--service-principal-client-id")]
    public string? ServicePrincipalClientId { get; set; }

    [CommandSwitch("--service-principal-client-secret")]
    public string? ServicePrincipalClientSecret { get; set; }

    [CommandSwitch("--service-principal-tenant-id")]
    public string? ServicePrincipalTenantId { get; set; }

    [CommandSwitch("--source")]
    public string? Source { get; set; }

    [CommandSwitch("--target-port")]
    public string? TargetPort { get; set; }

    [CommandSwitch("--token")]
    public string? Token { get; set; }

    [CommandSwitch("--workload-profile-name")]
    public string? WorkloadProfileName { get; set; }
}