using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
public class GcloudNetapp
{
    public GcloudNetapp(
        GcloudNetappActiveDirectories activeDirectories,
        GcloudNetappKmsConfigs kmsConfigs,
        GcloudNetappLocations locations,
        GcloudNetappOperations operations,
        GcloudNetappStoragePools storagePools,
        GcloudNetappVolumes volumes
    )
    {
        ActiveDirectories = activeDirectories;
        KmsConfigs = kmsConfigs;
        Locations = locations;
        Operations = operations;
        StoragePools = storagePools;
        Volumes = volumes;
    }

    public GcloudNetappActiveDirectories ActiveDirectories { get; }

    public GcloudNetappKmsConfigs KmsConfigs { get; }

    public GcloudNetappLocations Locations { get; }

    public GcloudNetappOperations Operations { get; }

    public GcloudNetappStoragePools StoragePools { get; }

    public GcloudNetappVolumes Volumes { get; }
}