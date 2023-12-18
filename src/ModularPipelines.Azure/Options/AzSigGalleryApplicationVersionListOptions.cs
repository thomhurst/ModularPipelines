using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sig", "gallery-application", "version", "list")]
public record AzSigGalleryApplicationVersionListOptions(
[property: CommandSwitch("--application-name")] string ApplicationName,
[property: CommandSwitch("--gallery-name")] string GalleryName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions;