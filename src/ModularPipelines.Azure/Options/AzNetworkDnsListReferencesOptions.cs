using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "dns", "list-references")]
public record AzNetworkDnsListReferencesOptions : AzOptions
{
    [CliOption("--parameters")]
    public string[]? Parameters { get; set; }
}