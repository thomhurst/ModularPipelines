using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("netappfiles", "volume")]
public class AzNetappfilesVolumeList
{
    public AzNetappfilesVolumeList(
        AzNetappfilesVolumeListNetappfilesPreview netappfilesPreview
    )
    {
        NetappfilesPreview = netappfilesPreview;
    }

    public AzNetappfilesVolumeListNetappfilesPreview NetappfilesPreview { get; }
}