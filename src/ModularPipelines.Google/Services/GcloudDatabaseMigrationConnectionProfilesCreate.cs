using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
[CliCommand("database-migration", "connection-profiles")]
public class GcloudDatabaseMigrationConnectionProfilesCreate
{
    public GcloudDatabaseMigrationConnectionProfilesCreate(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Alloydb(GcloudDatabaseMigrationConnectionProfilesCreateAlloydbOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Cloudsql(GcloudDatabaseMigrationConnectionProfilesCreateCloudsqlOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Mysql(GcloudDatabaseMigrationConnectionProfilesCreateMysqlOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Oracle(GcloudDatabaseMigrationConnectionProfilesCreateOracleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Postgresql(GcloudDatabaseMigrationConnectionProfilesCreatePostgresqlOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}