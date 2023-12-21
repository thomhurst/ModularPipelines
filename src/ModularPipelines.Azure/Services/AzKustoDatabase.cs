using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

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
        Create = create;
        Delete = delete;
        List = list;
        Show = show;
        Update = update;
        Wait = wait;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzKustoDatabaseCreate Create { get; }

    public AzKustoDatabaseDelete Delete { get; }

    public AzKustoDatabaseList List { get; }

    public AzKustoDatabaseShow Show { get; }

    public AzKustoDatabaseUpdate Update { get; }

    public AzKustoDatabaseWait Wait { get; }

    public async Task<CommandResult> AddPrincipal(AzKustoDatabaseAddPrincipalOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzKustoDatabaseAddPrincipalOptions(), token);
    }

    public async Task<CommandResult> ListPrincipal(AzKustoDatabaseListPrincipalOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RemovePrincipal(AzKustoDatabaseRemovePrincipalOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzKustoDatabaseRemovePrincipalOptions(), token);
    }
}