using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "hub", "memberships", "register")]
public record GcloudContainerHubMembershipsRegisterOptions(
[property: PositionalArgument] string MembershipName,
[property: PositionalArgument] string Location,
[property: CommandSwitch("--gke-cluster")] string GkeCluster,
[property: CommandSwitch("--gke-uri")] string GkeUri,
[property: CommandSwitch("--context")] string Context,
[property: CommandSwitch("--kubeconfig")] string Kubeconfig
) : GcloudOptions
{
    [BooleanCommandSwitch("--install-connect-agent")]
    public bool? InstallConnectAgent { get; set; }

    [BooleanCommandSwitch("--internal-ip")]
    public bool? InternalIp { get; set; }

    [CommandSwitch("--manifest-output-file")]
    public string? ManifestOutputFile { get; set; }

    [CommandSwitch("--proxy")]
    public string? Proxy { get; set; }

    [CommandSwitch("--service-account-key-file")]
    public string? ServiceAccountKeyFile { get; set; }

    [BooleanCommandSwitch("--enable-workload-identity")]
    public bool? EnableWorkloadIdentity { get; set; }

    [BooleanCommandSwitch("--has-private-issuer")]
    public bool? HasPrivateIssuer { get; set; }

    [CommandSwitch("--public-issuer-url")]
    public string? PublicIssuerUrl { get; set; }
}