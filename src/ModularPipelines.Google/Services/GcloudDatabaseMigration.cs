using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
public class GcloudDatabaseMigration
{
    public GcloudDatabaseMigration(
        GcloudDatabaseMigrationConnectionProfiles connectionProfiles,
        GcloudDatabaseMigrationConversionWorkspaces conversionWorkspaces,
        GcloudDatabaseMigrationMigrationJobs migrationJobs,
        GcloudDatabaseMigrationOperations operations,
        GcloudDatabaseMigrationPrivateConnections privateConnections
    )
    {
        ConnectionProfiles = connectionProfiles;
        ConversionWorkspaces = conversionWorkspaces;
        MigrationJobs = migrationJobs;
        Operations = operations;
        PrivateConnections = privateConnections;
    }

    public GcloudDatabaseMigrationConnectionProfiles ConnectionProfiles { get; }

    public GcloudDatabaseMigrationConversionWorkspaces ConversionWorkspaces { get; }

    public GcloudDatabaseMigrationMigrationJobs MigrationJobs { get; }

    public GcloudDatabaseMigrationOperations Operations { get; }

    public GcloudDatabaseMigrationPrivateConnections PrivateConnections { get; }
}