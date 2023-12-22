using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sphere", "image-package", "show")]
public record AzSphereImagePackageShowOptions(
[property: CommandSwitch("--image-package")] string ImagePackage
) : AzOptions;