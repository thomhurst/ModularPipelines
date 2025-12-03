using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "fleet", "memberships", "register")]
public record GcloudContainerFleetMembershipsRegisterOptions(
[property: CliArgument] string MembershipName,
[property: CliArgument] string Location,
[property: CliOption("--gke-cluster")] string GkeCluster,
[property: CliOption("--gke-uri")] string GkeUri,
[property: CliOption("--context")] string Context,
[property: CliOption("--kubeconfig")] string Kubeconfig
) : GcloudOptions
{
    [CliFlag("--install-connect-agent")]
    public bool? InstallConnectAgent { get; set; }

    [CliFlag("--internal-ip")]
    public bool? InternalIp { get; set; }

    [CliOption("--manifest-output-file")]
    public string? ManifestOutputFile { get; set; }

    [CliOption("--proxy")]
    public string? Proxy { get; set; }

    [CliOption("--service-account-key-file")]
    public string? ServiceAccountKeyFile { get; set; }

    [CliFlag("--enable-workload-identity")]
    public bool? EnableWorkloadIdentity { get; set; }

    [CliFlag("--has-private-issuer")]
    public bool? HasPrivateIssuer { get; set; }

    [CliOption("--public-issuer-url")]
    public string? PublicIssuerUrl { get; set; }
}