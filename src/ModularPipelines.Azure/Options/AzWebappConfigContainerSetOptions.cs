using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("webapp", "config", "container", "set")]
public record AzWebappConfigContainerSetOptions : AzOptions
{
    [CliOption("--docker-custom-image-name")]
    public string? DockerCustomImageName { get; set; }

    [CliOption("--docker-registry-server-password")]
    public string? DockerRegistryServerPassword { get; set; }

    [CliOption("--docker-registry-server-url")]
    public string? DockerRegistryServerUrl { get; set; }

    [CliOption("--docker-registry-server-user")]
    public string? DockerRegistryServerUser { get; set; }

    [CliFlag("--enable-app-service-storage")]
    public bool? EnableAppServiceStorage { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--multicontainer-config-file")]
    public string? MulticontainerConfigFile { get; set; }

    [CliOption("--multicontainer-config-type")]
    public string? MulticontainerConfigType { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--slot")]
    public string? Slot { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}