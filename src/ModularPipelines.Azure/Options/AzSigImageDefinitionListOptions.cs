using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("sig", "image-definition", "list")]
public record AzSigImageDefinitionListOptions(
[property: CliOption("--gallery-name")] string GalleryName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;