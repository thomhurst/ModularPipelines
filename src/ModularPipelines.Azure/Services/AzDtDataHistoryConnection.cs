using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("dt", "data-history")]
public class AzDtDataHistoryConnection
{
    public AzDtDataHistoryConnection(
        AzDtDataHistoryConnectionCreate create,
        ICommand internalCommand
    )
    {
        Create = create;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzDtDataHistoryConnectionCreate Create { get; }

    public async Task<CommandResult> Delete(AzDtDataHistoryConnectionDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzDtDataHistoryConnectionListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzDtDataHistoryConnectionShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Wait(AzDtDataHistoryConnectionWaitOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}