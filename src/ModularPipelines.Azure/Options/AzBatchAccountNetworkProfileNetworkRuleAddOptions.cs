using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("batch", "account", "network-profile", "network-rule", "add")]
public record AzBatchAccountNetworkProfileNetworkRuleAddOptions(
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--ip-address")]
    public string? IpAddress { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--profile")]
    public string? Profile { get; set; }
}