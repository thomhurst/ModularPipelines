using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("services", "peered-dns-domains", "list")]
public record GcloudServicesPeeredDnsDomainsListOptions(
[property: CommandSwitch("--network")] string Network
) : GcloudOptions
{
    [CommandSwitch("--service")]
    public string? Service { get; set; }
}