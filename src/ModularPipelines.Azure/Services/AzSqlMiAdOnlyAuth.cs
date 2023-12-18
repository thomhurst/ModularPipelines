using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sql", "mi")]
public class AzSqlMiAdOnlyAuth
{
    public AzSqlMiAdOnlyAuth(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Disable(AzSqlMiAdOnlyAuthDisableOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSqlMiAdOnlyAuthDisableOptions(), token);
    }

    public async Task<CommandResult> Enable(AzSqlMiAdOnlyAuthEnableOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSqlMiAdOnlyAuthEnableOptions(), token);
    }

    public async Task<CommandResult> Get(AzSqlMiAdOnlyAuthGetOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSqlMiAdOnlyAuthGetOptions(), token);
    }
}