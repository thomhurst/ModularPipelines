using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("database-migration")]
public class GcloudDatabaseMigrationConnectionProfiles
{
    public GcloudDatabaseMigrationConnectionProfiles(
        GcloudDatabaseMigrationConnectionProfilesCreate create,
        ICommand internalCommand
    )
    {
        Create = create;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public GcloudDatabaseMigrationConnectionProfilesCreate Create { get; }

    public async Task<CommandResult> Delete(GcloudDatabaseMigrationConnectionProfilesDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Describe(GcloudDatabaseMigrationConnectionProfilesDescribeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(GcloudDatabaseMigrationConnectionProfilesListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudDatabaseMigrationConnectionProfilesListOptions(), token);
    }

    public async Task<CommandResult> Update(GcloudDatabaseMigrationConnectionProfilesUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}