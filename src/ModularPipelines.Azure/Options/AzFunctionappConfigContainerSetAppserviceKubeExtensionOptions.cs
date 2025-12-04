using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("functionapp", "config", "container", "set", "(appservice-kube", "extension)")]
public record AzFunctionappConfigContainerSetAppserviceKubeExtensionOptions : AzOptions
{
    [CliOption("--docker-custom-image-name")]
    public string? DockerCustomImageName { get; set; }

    [CliOption("--docker-registry-server-password")]
    public string? DockerRegistryServerPassword { get; set; }

    [CliOption("--docker-registry-server-url")]
    public string? DockerRegistryServerUrl { get; set; }

    [CliOption("--docker-registry-server-user")]
    public string? DockerRegistryServerUser { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--slot")]
    public string? Slot { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}