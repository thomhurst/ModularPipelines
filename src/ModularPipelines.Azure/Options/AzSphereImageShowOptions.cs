using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sphere", "image", "show")]
public record AzSphereImageShowOptions(
[property: CommandSwitch("--catalog")] string Catalog,
[property: CommandSwitch("--image")] string Image,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions;