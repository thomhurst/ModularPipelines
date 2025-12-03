using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("services", "peered-dns-domains", "delete")]
public record GcloudServicesPeeredDnsDomainsDeleteOptions(
[property: CliArgument] string Name,
[property: CliOption("--network")] string Network
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--service")]
    public string? Service { get; set; }
}