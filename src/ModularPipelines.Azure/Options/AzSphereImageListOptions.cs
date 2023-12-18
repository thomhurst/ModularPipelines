using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sphere", "image", "list")]
public record AzSphereImageListOptions(
[property: CommandSwitch("--catalog")] string Catalog,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions;