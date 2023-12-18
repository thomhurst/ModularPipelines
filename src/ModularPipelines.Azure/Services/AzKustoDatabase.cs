using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kusto")]
public class AzKustoDatabase
{
    public AzKustoDatabase(
        AzKustoDatabaseCreate create,
        AzKustoDatabaseDelete delete,
        AzKustoDatabaseList list,
        AzKustoDatabaseShow show,
        AzKustoDatabaseUpdate update,
        AzKustoDatabaseWait wait,
        ICommand internalCommand
    )
    {
        CreateCommands = create;
        DeleteCommands = delete;
        ListCommands = list;
        ShowCommands = show;
        UpdateCommands = update;
        WaitCommands = wait;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzKustoDatabaseCreate CreateCommands { get; }

    public AzKustoDatabaseDelete DeleteCommands { get; }

    public AzKustoDatabaseList ListCommands { get; }

    public AzKustoDatabaseShow ShowCommands { get; }

    public AzKustoDatabaseUpdate UpdateCommands { get; }

    public AzKustoDatabaseWait WaitCommands { get; }

    public async Task<CommandResult> AddPrincipal(AzKustoDatabaseAddPrincipalOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Create(AzKustoDatabaseCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzKustoDatabaseDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzKustoDatabaseListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListPrincipal(AzKustoDatabaseListPrincipalOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RemovePrincipal(AzKustoDatabaseRemovePrincipalOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzKustoDatabaseShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(AzKustoDatabaseUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Wait(AzKustoDatabaseWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzKustoDatabaseWaitOptions(), token);
    }
}

