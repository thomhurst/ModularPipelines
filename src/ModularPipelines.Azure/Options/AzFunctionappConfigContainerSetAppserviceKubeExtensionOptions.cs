using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("functionapp", "config", "container", "set", "(appservice-kube", "extension)")]
public record AzFunctionappConfigContainerSetAppserviceKubeExtensionOptions : AzOptions
{
    [CommandSwitch("--docker-custom-image-name")]
    public string? DockerCustomImageName { get; set; }

    [CommandSwitch("--docker-registry-server-password")]
    public string? DockerRegistryServerPassword { get; set; }

    [CommandSwitch("--docker-registry-server-url")]
    public string? DockerRegistryServerUrl { get; set; }

    [CommandSwitch("--docker-registry-server-user")]
    public string? DockerRegistryServerUser { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--slot")]
    public string? Slot { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }
}

