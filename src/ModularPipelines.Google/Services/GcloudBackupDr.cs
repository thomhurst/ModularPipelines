using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
public class GcloudBackupDr
{
    public GcloudBackupDr(
        GcloudBackupDrLocations locations,
        GcloudBackupDrManagementServers managementServers,
        GcloudBackupDrOperations operations
    )
    {
        Locations = locations;
        ManagementServers = managementServers;
        Operations = operations;
    }

    public GcloudBackupDrLocations Locations { get; }

    public GcloudBackupDrManagementServers ManagementServers { get; }

    public GcloudBackupDrOperations Operations { get; }
}