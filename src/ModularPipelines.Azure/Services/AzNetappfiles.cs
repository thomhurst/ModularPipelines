using System.Diagnostics.CodeAnalysis;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
public class AzNetappfiles
{
    public AzNetappfiles(
        AzNetappfilesAccount account,
        AzNetappfilesPool pool,
        AzNetappfilesResource resource,
        AzNetappfilesSnapshot snapshot,
        AzNetappfilesSubvolume subvolume,
        AzNetappfilesVolume volume,
        AzNetappfilesVolumeGroup volumeGroup
    )
    {
        Account = account;
        Pool = pool;
        Resource = resource;
        Snapshot = snapshot;
        Subvolume = subvolume;
        Volume = volume;
        VolumeGroup = volumeGroup;
    }

    public AzNetappfilesAccount Account { get; }

    public AzNetappfilesPool Pool { get; }

    public AzNetappfilesResource Resource { get; }

    public AzNetappfilesSnapshot Snapshot { get; }

    public AzNetappfilesSubvolume Subvolume { get; }

    public AzNetappfilesVolume Volume { get; }

    public AzNetappfilesVolumeGroup VolumeGroup { get; }
}