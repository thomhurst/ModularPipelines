using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("webapp", "create")]
public record AzWebappCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--plan")] string Plan,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--assign-identity")]
    public string? AssignIdentity { get; set; }

    [CliOption("--deployment-container-image-name")]
    public string? DeploymentContainerImageName { get; set; }

    [CliOption("--deployment-local-git")]
    public string? DeploymentLocalGit { get; set; }

    [CliOption("--deployment-source-branch")]
    public string? DeploymentSourceBranch { get; set; }

    [CliOption("--deployment-source-url")]
    public string? DeploymentSourceUrl { get; set; }

    [CliOption("--docker-registry-server-password")]
    public string? DockerRegistryServerPassword { get; set; }

    [CliOption("--docker-registry-server-user")]
    public string? DockerRegistryServerUser { get; set; }

    [CliFlag("--https-only")]
    public bool? HttpsOnly { get; set; }

    [CliOption("--multicontainer-config-file")]
    public string? MulticontainerConfigFile { get; set; }

    [CliOption("--multicontainer-config-type")]
    public string? MulticontainerConfigType { get; set; }

    [CliOption("--public-network-access")]
    public string? PublicNetworkAccess { get; set; }

    [CliOption("--role")]
    public string? Role { get; set; }

    [CliOption("--runtime")]
    public string? Runtime { get; set; }

    [CliOption("--scope")]
    public string? Scope { get; set; }

    [CliOption("--startup-file")]
    public string? StartupFile { get; set; }

    [CliOption("--subnet")]
    public string? Subnet { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--vnet")]
    public string? Vnet { get; set; }
}