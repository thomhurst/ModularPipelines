using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("mysql", "server")]
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
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Delete(AzMysqlServerAdAdminDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMysqlServerAdAdminDeleteOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> List(AzMysqlServerAdAdminListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMysqlServerAdAdminListOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Show(AzMysqlServerAdAdminShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMysqlServerAdAdminShowOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Wait(AzMysqlServerAdAdminWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMysqlServerAdAdminWaitOptions(), cancellationToken: token);
    }
}