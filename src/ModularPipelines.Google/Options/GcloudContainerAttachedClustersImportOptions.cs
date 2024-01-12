using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "attached", "clusters", "import")]
public record GcloudContainerAttachedClustersImportOptions(
[property: CommandSwitch("--distribution")] string Distribution,
[property: CommandSwitch("--platform-version")] string PlatformVersion,
[property: CommandSwitch("--context")] string Context,
[property: CommandSwitch("--kubeconfig")] string Kubeconfig,
[property: CommandSwitch("--fleet-membership")] string FleetMembership,
[property: CommandSwitch("--fleet-membership-location")] string FleetMembershipLocation
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [BooleanCommandSwitch("--validate-only")]
    public bool? ValidateOnly { get; set; }

    [CommandSwitch("--proxy-secret-name")]
    public string? ProxySecretName { get; set; }

    [CommandSwitch("--proxy-secret-namespace")]
    public string? ProxySecretNamespace { get; set; }
}