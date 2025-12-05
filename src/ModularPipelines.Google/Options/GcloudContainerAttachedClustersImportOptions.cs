using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "attached", "clusters", "import")]
public record GcloudContainerAttachedClustersImportOptions(
[property: CliOption("--distribution")] string Distribution,
[property: CliOption("--platform-version")] string PlatformVersion,
[property: CliOption("--context")] string Context,
[property: CliOption("--kubeconfig")] string Kubeconfig,
[property: CliOption("--fleet-membership")] string FleetMembership,
[property: CliOption("--fleet-membership-location")] string FleetMembershipLocation
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliFlag("--validate-only")]
    public bool? ValidateOnly { get; set; }

    [CliOption("--proxy-secret-name")]
    public string? ProxySecretName { get; set; }

    [CliOption("--proxy-secret-namespace")]
    public string? ProxySecretNamespace { get; set; }
}