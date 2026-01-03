using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("datamigration")]
public class AzDatamigrationSqlService
{
    public AzDatamigrationSqlService(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzDatamigrationSqlServiceCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Delete(AzDatamigrationSqlServiceDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDatamigrationSqlServiceDeleteOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> DeleteNode(AzDatamigrationSqlServiceDeleteNodeOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDatamigrationSqlServiceDeleteNodeOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> List(AzDatamigrationSqlServiceListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDatamigrationSqlServiceListOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> ListAuthKey(AzDatamigrationSqlServiceListAuthKeyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> ListIntegrationRuntimeMetric(AzDatamigrationSqlServiceListIntegrationRuntimeMetricOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> ListMigration(AzDatamigrationSqlServiceListMigrationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> RegenerateAuthKey(AzDatamigrationSqlServiceRegenerateAuthKeyOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDatamigrationSqlServiceRegenerateAuthKeyOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Show(AzDatamigrationSqlServiceShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDatamigrationSqlServiceShowOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Update(AzDatamigrationSqlServiceUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDatamigrationSqlServiceUpdateOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Wait(AzDatamigrationSqlServiceWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDatamigrationSqlServiceWaitOptions(), cancellationToken: token);
    }
}