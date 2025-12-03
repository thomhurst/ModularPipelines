using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("webapp", "vnet-integration", "add")]
public record AzWebappVnetIntegrationAddOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--subnet")] string Subnet,
[property: CliOption("--vnet")] string Vnet
) : AzOptions
{
    [CliFlag("--skip-delegation-check")]
    public bool? SkipDelegationCheck { get; set; }

    [CliOption("--slot")]
    public string? Slot { get; set; }
}