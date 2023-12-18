using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sphere", "image-package", "show")]
public record AzSphereImagePackageShowOptions(
[property: CommandSwitch("--image-package")] string ImagePackage
) : AzOptions
{
}

