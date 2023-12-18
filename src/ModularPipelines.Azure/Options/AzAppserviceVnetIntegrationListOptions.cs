using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("appservice", "vnet-integration", "list")]
public record AzAppserviceVnetIntegrationListOptions(
[property: CommandSwitch("--plan")] string Plan,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions;