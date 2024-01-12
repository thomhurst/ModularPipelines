using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "hub", "memberships", "unregister")]
public record GcloudContainerHubMembershipsUnregisterOptions(
[property: PositionalArgument] string MembershipName,
[property: PositionalArgument] string Location,
[property: CommandSwitch("--gke-cluster")] string GkeCluster,
[property: CommandSwitch("--gke-uri")] string GkeUri,
[property: CommandSwitch("--context")] string Context,
[property: CommandSwitch("--kubeconfig")] string Kubeconfig
) : GcloudOptions
{
    [BooleanCommandSwitch("--uninstall-connect-agent")]
    public bool? UninstallConnectAgent { get; set; }
}