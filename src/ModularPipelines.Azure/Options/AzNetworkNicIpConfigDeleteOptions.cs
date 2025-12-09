using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "nic", "ip-config", "delete")]
public record AzNetworkNicIpConfigDeleteOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--nic-name")] string NicName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }
}