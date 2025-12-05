using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("postgres", "server")]
public class AzPostgresServerAdAdmin
{
    public AzPostgresServerAdAdmin(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzPostgresServerAdAdminCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzPostgresServerAdAdminDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzPostgresServerAdAdminDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzPostgresServerAdAdminListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzPostgresServerAdAdminListOptions(), token);
    }

    public async Task<CommandResult> Show(AzPostgresServerAdAdminShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzPostgresServerAdAdminShowOptions(), token);
    }

    public async Task<CommandResult> Wait(AzPostgresServerAdAdminWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzPostgresServerAdAdminWaitOptions(), token);
    }
}