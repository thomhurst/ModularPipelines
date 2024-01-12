using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("active-directory", "domains", "trusts", "create")]
public record GcloudActiveDirectoryDomainsTrustsCreateOptions(
[property: PositionalArgument] string Domain,
[property: CommandSwitch("--target-dns-ip-addresses")] string[] TargetDnsIpAddresses,
[property: CommandSwitch("--target-domain-name")] string TargetDomainName
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--direction")]
    public string? Direction { get; set; }

    [CommandSwitch("--handshake-secret")]
    public string? HandshakeSecret { get; set; }

    [BooleanCommandSwitch("--selective-authentication")]
    public bool? SelectiveAuthentication { get; set; }

    [CommandSwitch("--type")]
    public string? Type { get; set; }
}