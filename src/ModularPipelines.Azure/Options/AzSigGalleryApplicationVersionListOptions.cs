using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("sig", "gallery-application", "version", "list")]
public record AzSigGalleryApplicationVersionListOptions(
[property: CliOption("--application-name")] string ApplicationName,
[property: CliOption("--gallery-name")] string GalleryName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;