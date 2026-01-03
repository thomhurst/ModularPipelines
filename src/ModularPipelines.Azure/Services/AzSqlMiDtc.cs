using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("sql", "mi")]
public class AzSqlMiDtc
{
    public AzSqlMiDtc(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Show(AzSqlMiDtcShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSqlMiDtcShowOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Update(AzSqlMiDtcUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSqlMiDtcUpdateOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Wait(AzSqlMiDtcWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSqlMiDtcWaitOptions(), cancellationToken: token);
    }
}