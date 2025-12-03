using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("active-directory", "domains", "trusts", "create")]
public record GcloudActiveDirectoryDomainsTrustsCreateOptions(
[property: CliArgument] string Domain,
[property: CliOption("--target-dns-ip-addresses")] string[] TargetDnsIpAddresses,
[property: CliOption("--target-domain-name")] string TargetDomainName
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--direction")]
    public string? Direction { get; set; }

    [CliOption("--handshake-secret")]
    public string? HandshakeSecret { get; set; }

    [CliFlag("--selective-authentication")]
    public bool? SelectiveAuthentication { get; set; }

    [CliOption("--type")]
    public string? Type { get; set; }
}