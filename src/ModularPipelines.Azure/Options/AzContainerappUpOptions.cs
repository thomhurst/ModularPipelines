using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("containerapp", "up")]
public record AzContainerappUpOptions(
[property: CliOption("--name")] string Name
) : AzOptions
{
    [CliOption("--artifact")]
    public string? Artifact { get; set; }

    [CliOption("--branch")]
    public string? Branch { get; set; }

    [CliFlag("--browse")]
    public bool? Browse { get; set; }

    [CliOption("--connected-cluster-id")]
    public string? ConnectedClusterId { get; set; }

    [CliOption("--context-path")]
    public string? ContextPath { get; set; }

    [CliOption("--custom-location")]
    public string? CustomLocation { get; set; }

    [CliOption("--env-vars")]
    public string? EnvVars { get; set; }

    [CliOption("--environment")]
    public string? Environment { get; set; }

    [CliOption("--image")]
    public string? Image { get; set; }

    [CliOption("--ingress")]
    public string? Ingress { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--logs-workspace-id")]
    public string? LogsWorkspaceId { get; set; }

    [CliOption("--logs-workspace-key")]
    public string? LogsWorkspaceKey { get; set; }

    [CliOption("--registry-password")]
    public string? RegistryPassword { get; set; }

    [CliOption("--registry-server")]
    public string? RegistryServer { get; set; }

    [CliOption("--registry-username")]
    public string? RegistryUsername { get; set; }

    [CliOption("--repo")]
    public string? Repo { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--service-principal-client-id")]
    public string? ServicePrincipalClientId { get; set; }

    [CliOption("--service-principal-client-secret")]
    public string? ServicePrincipalClientSecret { get; set; }

    [CliOption("--service-principal-tenant-id")]
    public string? ServicePrincipalTenantId { get; set; }

    [CliOption("--source")]
    public string? Source { get; set; }

    [CliOption("--target-port")]
    public string? TargetPort { get; set; }

    [CliOption("--token")]
    public string? Token { get; set; }

    [CliOption("--workload-profile-name")]
    public string? WorkloadProfileName { get; set; }
}