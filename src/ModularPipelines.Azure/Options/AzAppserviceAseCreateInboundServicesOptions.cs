using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("appservice", "ase", "create-inbound-services")]
public record AzAppserviceAseCreateInboundServicesOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--subnet")] string Subnet
) : AzOptions
{
    [CommandSwitch("--vnet-name")]
    public string? VnetName { get; set; }
}