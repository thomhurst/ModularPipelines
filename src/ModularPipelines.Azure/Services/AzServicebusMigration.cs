using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("servicebus")]
public class AzServicebusMigration
{
    public AzServicebusMigration(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Abort(AzServicebusMigrationAbortOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzServicebusMigrationAbortOptions(), token);
    }

    public async Task<CommandResult> Complete(AzServicebusMigrationCompleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzServicebusMigrationCompleteOptions(), token);
    }

    public async Task<CommandResult> Delete(AzServicebusMigrationDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzServicebusMigrationDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzServicebusMigrationListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzServicebusMigrationShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzServicebusMigrationShowOptions(), token);
    }

    public async Task<CommandResult> Start(AzServicebusMigrationStartOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzServicebusMigrationStartOptions(), token);
    }

    public async Task<CommandResult> Wait(AzServicebusMigrationWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzServicebusMigrationWaitOptions(), token);
    }
}