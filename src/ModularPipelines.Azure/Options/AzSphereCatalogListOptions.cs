using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sphere", "catalog", "list")]
public record AzSphereCatalogListOptions(
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions;