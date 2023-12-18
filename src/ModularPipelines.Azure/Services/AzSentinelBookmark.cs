using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sentinel")]
public class AzSentinelBookmark
{
    public AzSentinelBookmark(
        AzSentinelBookmarkRelation relation,
        ICommand internalCommand
    )
    {
        Relation = relation;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzSentinelBookmarkRelation Relation { get; }

    public async Task<CommandResult> Create(AzSentinelBookmarkCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzSentinelBookmarkDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Expand(AzSentinelBookmarkExpandOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzSentinelBookmarkListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzSentinelBookmarkShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSentinelBookmarkShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzSentinelBookmarkUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSentinelBookmarkUpdateOptions(), token);
    }
}