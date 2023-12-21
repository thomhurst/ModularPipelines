using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sql", "server")]
public class AzSqlServerAdOnlyAuth
{
    public AzSqlServerAdOnlyAuth(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Disable(AzSqlServerAdOnlyAuthDisableOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSqlServerAdOnlyAuthDisableOptions(), token);
    }

    public async Task<CommandResult> Enable(AzSqlServerAdOnlyAuthEnableOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSqlServerAdOnlyAuthEnableOptions(), token);
    }

    public async Task<CommandResult> Get(AzSqlServerAdOnlyAuthGetOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSqlServerAdOnlyAuthGetOptions(), token);
    }
}