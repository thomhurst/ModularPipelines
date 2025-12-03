using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cognitiveservices", "account", "network-rule", "add")]
public record AzCognitiveservicesAccountNetworkRuleAddOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--ip-address")]
    public string? IpAddress { get; set; }

    [CliOption("--subnet")]
    public string? Subnet { get; set; }

    [CliOption("--vnet-name")]
    public string? VnetName { get; set; }
}