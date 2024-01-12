using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("services", "peered-dns-domains", "create")]
public record GcloudServicesPeeredDnsDomainsCreateOptions(
[property: PositionalArgument] string Name,
[property: CommandSwitch("--dns-suffix")] string DnsSuffix,
[property: CommandSwitch("--network")] string Network
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--service")]
    public string? Service { get; set; }
}