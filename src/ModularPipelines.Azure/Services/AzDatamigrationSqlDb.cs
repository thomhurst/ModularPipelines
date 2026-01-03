using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("datamigration")]
public class AzDatamigrationSqlDb
{
    public AzDatamigrationSqlDb(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Cancel(AzDatamigrationSqlDbCancelOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Create(AzDatamigrationSqlDbCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Delete(AzDatamigrationSqlDbDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDatamigrationSqlDbDeleteOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Show(AzDatamigrationSqlDbShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDatamigrationSqlDbShowOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Wait(AzDatamigrationSqlDbWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDatamigrationSqlDbWaitOptions(), cancellationToken: token);
    }
}