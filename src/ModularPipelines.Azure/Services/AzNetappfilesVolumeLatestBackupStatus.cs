using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("netappfiles", "volume")]
public class AzNetappfilesVolumeLatestBackupStatus
{
    public AzNetappfilesVolumeLatestBackupStatus(
        AzNetappfilesVolumeLatestBackupStatusCurrent current
    )
    {
        Current = current;
    }

    public AzNetappfilesVolumeLatestBackupStatusCurrent Current { get; }
}