using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("netappfiles", "volume")]
public class AzNetappfilesVolumeCreate
{
    public AzNetappfilesVolumeCreate(
        AzNetappfilesVolumeCreateNetappfilesPreview netappfilesPreview
    )
    {
        NetappfilesPreview = netappfilesPreview;
    }

    public AzNetappfilesVolumeCreateNetappfilesPreview NetappfilesPreview { get; }
}