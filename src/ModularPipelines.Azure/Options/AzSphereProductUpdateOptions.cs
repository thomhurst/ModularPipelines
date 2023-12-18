using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sphere", "product", "update")]
public record AzSphereProductUpdateOptions(
[property: CommandSwitch("--catalog")] string Catalog,
[property: CommandSwitch("--description")] string Description,
[property: CommandSwitch("--product")] string Product,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions;