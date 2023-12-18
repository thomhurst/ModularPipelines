using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("netappfiles", "volume")]
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

