using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("acr", "network-rule", "add")]
public record AzAcrNetworkRuleAddOptions(
[property: CliOption("--name")] string Name
) : AzOptions
{
    [CliOption("--ip-address")]
    public string? IpAddress { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}