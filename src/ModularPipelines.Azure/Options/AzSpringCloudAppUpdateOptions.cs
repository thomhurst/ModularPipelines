using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("spring-cloud", "app", "update")]
public record AzSpringCloudAppUpdateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--service")] string Service
) : AzOptions
{
    [BooleanCommandSwitch("--assign-endpoint")]
    public bool? AssignEndpoint { get; set; }

    [CommandSwitch("--config-file-patterns")]
    public string? ConfigFilePatterns { get; set; }

    [CommandSwitch("--deployment")]
    public string? Deployment { get; set; }

    [BooleanCommandSwitch("--disable-probe")]
    public bool? DisableProbe { get; set; }

    [BooleanCommandSwitch("--enable-ingress-to-app-tls")]
    public bool? EnableIngressToAppTls { get; set; }

    [BooleanCommandSwitch("--enable-persistent-storage")]
    public bool? EnablePersistentStorage { get; set; }

    [CommandSwitch("--env")]
    public string? Env { get; set; }

    [BooleanCommandSwitch("--https-only")]
    public bool? HttpsOnly { get; set; }

    [CommandSwitch("--jvm-options")]
    public string? JvmOptions { get; set; }

    [CommandSwitch("--loaded-public-certificate-file")]
    public string? LoadedPublicCertificateFile { get; set; }

    [CommandSwitch("--main-entry")]
    public string? MainEntry { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--persistent-storage")]
    public string? PersistentStorage { get; set; }

    [CommandSwitch("--runtime-version")]
    public string? RuntimeVersion { get; set; }
}