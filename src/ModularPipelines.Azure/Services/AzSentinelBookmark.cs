using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("sentinel")]
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

    public async Task<CommandResult> Delete(AzSentinelBookmarkDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSentinelBookmarkDeleteOptions(), token);
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