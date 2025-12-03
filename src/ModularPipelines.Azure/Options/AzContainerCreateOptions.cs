using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "create")]
public record AzContainerCreateOptions(
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--acr-identity")]
    public string? AcrIdentity { get; set; }

    [CliOption("--add-capabilities")]
    public string? AddCapabilities { get; set; }

    [CliFlag("--allow-escalation")]
    public bool? AllowEscalation { get; set; }

    [CliOption("--assign-identity")]
    public string? AssignIdentity { get; set; }

    [CliOption("--azure-file-volume-account-key")]
    public int? AzureFileVolumeAccountKey { get; set; }

    [CliOption("--azure-file-volume-account-name")]
    public int? AzureFileVolumeAccountName { get; set; }

    [CliOption("--azure-file-volume-mount-path")]
    public string? AzureFileVolumeMountPath { get; set; }

    [CliOption("--azure-file-volume-share-name")]
    public string? AzureFileVolumeShareName { get; set; }

    [CliOption("--cce-policy")]
    public string? CcePolicy { get; set; }

    [CliOption("--command-line")]
    public string? CommandLine { get; set; }

    [CliOption("--cpu")]
    public string? Cpu { get; set; }

    [CliOption("--dns-name-label")]
    public string? DnsNameLabel { get; set; }

    [CliOption("--drop-capabilities")]
    public string? DropCapabilities { get; set; }

    [CliOption("--environment-variables")]
    public string? ContainerCreateEnvironmentVariables { get; set; }

    [CliOption("--file")]
    public string? File { get; set; }

    [CliOption("--gitrepo-dir")]
    public string? GitrepoDir { get; set; }

    [CliOption("--gitrepo-mount-path")]
    public string? GitrepoMountPath { get; set; }

    [CliOption("--gitrepo-revision")]
    public string? GitrepoRevision { get; set; }

    [CliOption("--gitrepo-url")]
    public string? GitrepoUrl { get; set; }

    [CliOption("--image")]
    public string? Image { get; set; }

    [CliOption("--ip-address")]
    public string? IpAddress { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--log-analytics-workspace")]
    public string? LogAnalyticsWorkspace { get; set; }

    [CliOption("--log-analytics-workspace-key")]
    public string? LogAnalyticsWorkspaceKey { get; set; }

    [CliOption("--memory")]
    public string? Memory { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--os-type")]
    public string? OsType { get; set; }

    [CliOption("--ports")]
    public string? Ports { get; set; }

    [CliOption("--priority")]
    public string? Priority { get; set; }

    [CliFlag("--privileged")]
    public bool? Privileged { get; set; }

    [CliOption("--protocol")]
    public string? Protocol { get; set; }

    [CliOption("--registry-login-server")]
    public string? RegistryLoginServer { get; set; }

    [CliOption("--registry-password")]
    public string? RegistryPassword { get; set; }

    [CliOption("--registry-username")]
    public string? RegistryUsername { get; set; }

    [CliOption("--restart-policy")]
    public string? RestartPolicy { get; set; }

    [CliOption("--role")]
    public string? Role { get; set; }

    [CliOption("--run-as-group")]
    public string? RunAsGroup { get; set; }

    [CliOption("--run-as-user")]
    public string? RunAsUser { get; set; }

    [CliOption("--scope")]
    public string? Scope { get; set; }

    [CliOption("--seccomp-profile")]
    public string? SeccompProfile { get; set; }

    [CliOption("--secrets")]
    public string? Secrets { get; set; }

    [CliOption("--secrets-mount-path")]
    public string? SecretsMountPath { get; set; }

    [CliOption("--secure-environment-variables")]
    public string? SecureEnvironmentVariables { get; set; }

    [CliOption("--sku")]
    public string? Sku { get; set; }

    [CliOption("--subnet")]
    public string? Subnet { get; set; }

    [CliOption("--subnet-address-prefix")]
    public string? SubnetAddressPrefix { get; set; }

    [CliOption("--vnet")]
    public string? Vnet { get; set; }

    [CliOption("--vnet-address-prefix")]
    public string? VnetAddressPrefix { get; set; }

    [CliOption("--zone")]
    public string? Zone { get; set; }
}