using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("webapp", "deployment", "slot", "create")]
public record AzWebappDeploymentSlotCreateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--slot")] string Slot
) : AzOptions
{
    [CommandSwitch("--configuration-source")]
    public string? ConfigurationSource { get; set; }

    [CommandSwitch("--deployment-container-image-name")]
    public string? DeploymentContainerImageName { get; set; }

    [CommandSwitch("--docker-registry-server-password")]
    public string? DockerRegistryServerPassword { get; set; }

    [CommandSwitch("--docker-registry-server-user")]
    public string? DockerRegistryServerUser { get; set; }
}