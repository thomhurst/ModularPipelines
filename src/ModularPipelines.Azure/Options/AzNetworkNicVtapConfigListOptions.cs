using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "nic", "vtap-config", "list")]
public record AzNetworkNicVtapConfigListOptions(
[property: CliOption("--nic-name")] string NicName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;