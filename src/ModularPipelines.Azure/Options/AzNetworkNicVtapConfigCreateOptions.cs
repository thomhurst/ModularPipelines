using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "nic", "vtap-config", "create")]
public record AzNetworkNicVtapConfigCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--nic-name")] string NicName,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--vnet-tap")] string VnetTap
) : AzOptions;