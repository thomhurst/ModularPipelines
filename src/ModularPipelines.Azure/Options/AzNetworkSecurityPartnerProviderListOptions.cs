using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "security-partner-provider", "list")]
public record AzNetworkSecurityPartnerProviderListOptions : AzOptions
{
    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}