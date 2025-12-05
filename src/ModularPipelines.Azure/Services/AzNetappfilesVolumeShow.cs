using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("netappfiles", "volume")]
public class AzNetappfilesVolumeShow
{
    public AzNetappfilesVolumeShow(
        AzNetappfilesVolumeShowNetappfilesPreview netappfilesPreview
    )
    {
        NetappfilesPreview = netappfilesPreview;
    }

    public AzNetappfilesVolumeShowNetappfilesPreview NetappfilesPreview { get; }
}