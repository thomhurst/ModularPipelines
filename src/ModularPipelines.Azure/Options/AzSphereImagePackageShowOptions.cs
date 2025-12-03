using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sphere", "image-package", "show")]
public record AzSphereImagePackageShowOptions(
[property: CliOption("--image-package")] string ImagePackage
) : AzOptions;