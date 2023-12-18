using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sphere", "catalog", "list")]
public record AzSphereCatalogListOptions(
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
}