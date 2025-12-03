using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("webapp", "deployment", "slot", "create")]
public record AzWebappDeploymentSlotCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--slot")] string Slot
) : AzOptions
{
    [CliOption("--configuration-source")]
    public string? ConfigurationSource { get; set; }

    [CliOption("--deployment-container-image-name")]
    public string? DeploymentContainerImageName { get; set; }

    [CliOption("--docker-registry-server-password")]
    public string? DockerRegistryServerPassword { get; set; }

    [CliOption("--docker-registry-server-user")]
    public string? DockerRegistryServerUser { get; set; }
}