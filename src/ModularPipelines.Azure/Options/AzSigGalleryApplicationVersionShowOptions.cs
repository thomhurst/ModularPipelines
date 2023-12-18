using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sig", "gallery-application", "version", "show")]
public record AzSigGalleryApplicationVersionShowOptions(
[property: CommandSwitch("--application-name")] string ApplicationName,
[property: CommandSwitch("--gallery-name")] string GalleryName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--package-file-link")] string PackageFileLink,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--expand")]
    public string? Expand { get; set; }

    [CommandSwitch("--gallery-application-version-name")]
    public string? GalleryApplicationVersionName { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }
}