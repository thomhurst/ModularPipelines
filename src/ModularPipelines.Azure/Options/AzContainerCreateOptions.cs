using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "create")]
public record AzContainerCreateOptions(
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--acr-identity")]
    public string? AcrIdentity { get; set; }

    [CommandSwitch("--add-capabilities")]
    public string? AddCapabilities { get; set; }

    [BooleanCommandSwitch("--allow-escalation")]
    public bool? AllowEscalation { get; set; }

    [CommandSwitch("--assign-identity")]
    public string? AssignIdentity { get; set; }

    [CommandSwitch("--azure-file-volume-account-key")]
    public int? AzureFileVolumeAccountKey { get; set; }

    [CommandSwitch("--azure-file-volume-account-name")]
    public int? AzureFileVolumeAccountName { get; set; }

    [CommandSwitch("--azure-file-volume-mount-path")]
    public string? AzureFileVolumeMountPath { get; set; }

    [CommandSwitch("--azure-file-volume-share-name")]
    public string? AzureFileVolumeShareName { get; set; }

    [CommandSwitch("--cce-policy")]
    public string? CcePolicy { get; set; }

    [CommandSwitch("--command-line")]
    public string? CommandLine { get; set; }

    [CommandSwitch("--cpu")]
    public string? Cpu { get; set; }

    [CommandSwitch("--dns-name-label")]
    public string? DnsNameLabel { get; set; }

    [CommandSwitch("--drop-capabilities")]
    public string? DropCapabilities { get; set; }

    [CommandSwitch("--environment-variables")]
    public string? EnvironmentVariables { get; set; }

    [CommandSwitch("--file")]
    public string? File { get; set; }

    [CommandSwitch("--gitrepo-dir")]
    public string? GitrepoDir { get; set; }

    [CommandSwitch("--gitrepo-mount-path")]
    public string? GitrepoMountPath { get; set; }

    [CommandSwitch("--gitrepo-revision")]
    public string? GitrepoRevision { get; set; }

    [CommandSwitch("--gitrepo-url")]
    public string? GitrepoUrl { get; set; }

    [CommandSwitch("--image")]
    public string? Image { get; set; }

    [CommandSwitch("--ip-address")]
    public string? IpAddress { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--log-analytics-workspace")]
    public string? LogAnalyticsWorkspace { get; set; }

    [CommandSwitch("--log-analytics-workspace-key")]
    public string? LogAnalyticsWorkspaceKey { get; set; }

    [CommandSwitch("--memory")]
    public string? Memory { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--os-type")]
    public string? OsType { get; set; }

    [CommandSwitch("--ports")]
    public string? Ports { get; set; }

    [CommandSwitch("--priority")]
    public string? Priority { get; set; }

    [BooleanCommandSwitch("--privileged")]
    public bool? Privileged { get; set; }

    [CommandSwitch("--protocol")]
    public string? Protocol { get; set; }

    [CommandSwitch("--registry-login-server")]
    public string? RegistryLoginServer { get; set; }

    [CommandSwitch("--registry-password")]
    public string? RegistryPassword { get; set; }

    [CommandSwitch("--registry-username")]
    public string? RegistryUsername { get; set; }

    [CommandSwitch("--restart-policy")]
    public string? RestartPolicy { get; set; }

    [CommandSwitch("--role")]
    public string? Role { get; set; }

    [CommandSwitch("--run-as-group")]
    public string? RunAsGroup { get; set; }

    [CommandSwitch("--run-as-user")]
    public string? RunAsUser { get; set; }

    [CommandSwitch("--scope")]
    public string? Scope { get; set; }

    [CommandSwitch("--seccomp-profile")]
    public string? SeccompProfile { get; set; }

    [CommandSwitch("--secrets")]
    public string? Secrets { get; set; }

    [CommandSwitch("--secrets-mount-path")]
    public string? SecretsMountPath { get; set; }

    [CommandSwitch("--secure-environment-variables")]
    public string? SecureEnvironmentVariables { get; set; }

    [CommandSwitch("--sku")]
    public string? Sku { get; set; }

    [CommandSwitch("--subnet")]
    public string? Subnet { get; set; }

    [CommandSwitch("--subnet-address-prefix")]
    public string? SubnetAddressPrefix { get; set; }

    [CommandSwitch("--vnet")]
    public string? Vnet { get; set; }

    [CommandSwitch("--vnet-address-prefix")]
    public string? VnetAddressPrefix { get; set; }

    [CommandSwitch("--vnet-name")]
    public string? VnetName { get; set; }

    [CommandSwitch("--zone")]
    public string? Zone { get; set; }
}