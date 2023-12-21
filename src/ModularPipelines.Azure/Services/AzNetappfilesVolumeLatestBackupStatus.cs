using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

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