using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sig", "gallery-application", "show")]
public record AzSigGalleryApplicationShowOptions : AzOptions
{
    [CommandSwitch("--application-name")]
    public string? ApplicationName { get; set; }

    [CommandSwitch("--gallery-name")]
    public string? GalleryName { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }
}