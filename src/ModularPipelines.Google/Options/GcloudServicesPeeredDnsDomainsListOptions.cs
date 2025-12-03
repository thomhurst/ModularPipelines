using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("services", "peered-dns-domains", "list")]
public record GcloudServicesPeeredDnsDomainsListOptions(
[property: CliOption("--network")] string Network
) : GcloudOptions
{
    [CliOption("--service")]
    public string? Service { get; set; }
}