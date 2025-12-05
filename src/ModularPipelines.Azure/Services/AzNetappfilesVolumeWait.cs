using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("netappfiles", "volume")]
public class AzNetappfilesVolumeWait
{
    public AzNetappfilesVolumeWait(
        AzNetappfilesVolumeWaitNetappfilesPreview netappfilesPreview
    )
    {
        NetappfilesPreview = netappfilesPreview;
    }

    public AzNetappfilesVolumeWaitNetappfilesPreview NetappfilesPreview { get; }
}