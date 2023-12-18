using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sig", "share", "enable-community", "(image-gallery", "extension)")]
public record AzSigShareEnableCommunityImageGalleryExtensionOptions : AzOptions
{
    [CommandSwitch("--gallery-name")]
    public string? GalleryName { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }

    [CommandSwitch("--subscription-ids")]
    public string? SubscriptionIds { get; set; }

    [CommandSwitch("--tenant-ids")]
    public string? TenantIds { get; set; }
}