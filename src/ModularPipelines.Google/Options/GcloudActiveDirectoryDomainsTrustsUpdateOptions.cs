using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("active-directory", "domains", "trusts", "update")]
public record GcloudActiveDirectoryDomainsTrustsUpdateOptions(
[property: PositionalArgument] string Domain,
[property: CommandSwitch("--target-dns-ip-addresses")] string[] TargetDnsIpAddresses,
[property: CommandSwitch("--target-domain-name")] string TargetDomainName
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }
}