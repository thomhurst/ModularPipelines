using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
public class AzGraph
{
    public AzGraph(
        AzGraphSharedQuery sharedQuery,
        ICommand internalCommand
    )
    {
        SharedQuery = sharedQuery;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzGraphSharedQuery SharedQuery { get; }

    public async Task<CommandResult> Query(AzGraphQueryOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}