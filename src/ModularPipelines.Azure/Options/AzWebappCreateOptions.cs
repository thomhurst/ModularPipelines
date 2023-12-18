using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("webapp", "create")]
public record AzWebappCreateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--plan")] string Plan,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--assign-identity")]
    public string? AssignIdentity { get; set; }

    [CommandSwitch("--deployment-container-image-name")]
    public string? DeploymentContainerImageName { get; set; }

    [CommandSwitch("--deployment-local-git")]
    public string? DeploymentLocalGit { get; set; }

    [CommandSwitch("--deployment-source-branch")]
    public string? DeploymentSourceBranch { get; set; }

    [CommandSwitch("--deployment-source-url")]
    public string? DeploymentSourceUrl { get; set; }

    [CommandSwitch("--docker-registry-server-password")]
    public string? DockerRegistryServerPassword { get; set; }

    [CommandSwitch("--docker-registry-server-user")]
    public string? DockerRegistryServerUser { get; set; }

    [BooleanCommandSwitch("--https-only")]
    public bool? HttpsOnly { get; set; }

    [CommandSwitch("--multicontainer-config-file")]
    public string? MulticontainerConfigFile { get; set; }

    [CommandSwitch("--multicontainer-config-type")]
    public string? MulticontainerConfigType { get; set; }

    [CommandSwitch("--public-network-access")]
    public string? PublicNetworkAccess { get; set; }

    [CommandSwitch("--role")]
    public string? Role { get; set; }

    [CommandSwitch("--runtime")]
    public string? Runtime { get; set; }

    [CommandSwitch("--scope")]
    public string? Scope { get; set; }

    [CommandSwitch("--startup-file")]
    public string? StartupFile { get; set; }

    [CommandSwitch("--subnet")]
    public string? Subnet { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--vnet")]
    public string? Vnet { get; set; }
}