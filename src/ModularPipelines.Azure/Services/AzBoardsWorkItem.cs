using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("boards")]
public class AzBoardsWorkItem
{
    public AzBoardsWorkItem(
        AzBoardsWorkItemRelation relation,
        ICommand internalCommand
    )
    {
        Relation = relation;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzBoardsWorkItemRelation Relation { get; }

    public async Task<CommandResult> Create(AzBoardsWorkItemCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzBoardsWorkItemDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzBoardsWorkItemShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(AzBoardsWorkItemUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}