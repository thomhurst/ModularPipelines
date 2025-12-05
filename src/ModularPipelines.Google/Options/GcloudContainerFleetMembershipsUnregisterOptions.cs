using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "fleet", "memberships", "unregister")]
public record GcloudContainerFleetMembershipsUnregisterOptions(
[property: CliArgument] string MembershipName,
[property: CliArgument] string Location,
[property: CliOption("--gke-cluster")] string GkeCluster,
[property: CliOption("--gke-uri")] string GkeUri,
[property: CliOption("--context")] string Context,
[property: CliOption("--kubeconfig")] string Kubeconfig
) : GcloudOptions
{
    [CliFlag("--uninstall-connect-agent")]
    public bool? UninstallConnectAgent { get; set; }
}