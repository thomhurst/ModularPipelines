using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
public class AzBoards
{
    public AzBoards(
        AzBoardsArea area,
        AzBoardsIteration iteration,
        AzBoardsWorkItem workItem,
        ICommand internalCommand
    )
    {
        Area = area;
        Iteration = iteration;
        WorkItem = workItem;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzBoardsArea Area { get; }

    public AzBoardsIteration Iteration { get; }

    public AzBoardsWorkItem WorkItem { get; }

    public async Task<CommandResult> Query(AzBoardsQueryOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzBoardsQueryOptions(), token);
    }
}