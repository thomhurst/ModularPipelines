using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("services", "peered-dns-domains", "delete")]
public record GcloudServicesPeeredDnsDomainsDeleteOptions(
[property: PositionalArgument] string Name,
[property: CommandSwitch("--network")] string Network
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--service")]
    public string? Service { get; set; }
}