using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("active-directory", "domains", "trusts", "update")]
public record GcloudActiveDirectoryDomainsTrustsUpdateOptions(
[property: CliArgument] string Domain,
[property: CliOption("--target-dns-ip-addresses")] string[] TargetDnsIpAddresses,
[property: CliOption("--target-domain-name")] string TargetDomainName
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }
}