using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;
using ModularPipelines.Azure.Options;

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