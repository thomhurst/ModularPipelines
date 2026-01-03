using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("sql")]
public class AzSqlDw
{
    public AzSqlDw(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzSqlDwCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Delete(AzSqlDwDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSqlDwDeleteOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> List(AzSqlDwListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSqlDwListOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Pause(AzSqlDwPauseOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSqlDwPauseOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Resume(AzSqlDwResumeOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSqlDwResumeOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Show(AzSqlDwShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSqlDwShowOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Update(AzSqlDwUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSqlDwUpdateOptions(), cancellationToken: token);
    }
}