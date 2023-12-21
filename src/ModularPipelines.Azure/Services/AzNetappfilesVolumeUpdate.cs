using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("netappfiles", "volume")]
public class AzNetappfilesVolumeUpdate
{
    public AzNetappfilesVolumeUpdate(
        AzNetappfilesVolumeUpdateNetappfilesPreview netappfilesPreview
    )
    {
        NetappfilesPreview = netappfilesPreview;
    }

    public AzNetappfilesVolumeUpdateNetappfilesPreview NetappfilesPreview { get; }
}