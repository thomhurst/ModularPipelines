using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mysql", "server")]
public class AzMysqlServerAdAdmin
{
    public AzMysqlServerAdAdmin(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzMysqlServerAdAdminCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzMysqlServerAdAdminDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMysqlServerAdAdminDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzMysqlServerAdAdminListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMysqlServerAdAdminListOptions(), token);
    }

    public async Task<CommandResult> Show(AzMysqlServerAdAdminShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMysqlServerAdAdminShowOptions(), token);
    }

    public async Task<CommandResult> Wait(AzMysqlServerAdAdminWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMysqlServerAdAdminWaitOptions(), token);
    }
}

