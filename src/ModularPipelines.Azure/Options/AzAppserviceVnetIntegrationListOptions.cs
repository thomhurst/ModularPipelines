using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("appservice", "vnet-integration", "list")]
public record AzAppserviceVnetIntegrationListOptions(
[property: CliOption("--plan")] string Plan,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;