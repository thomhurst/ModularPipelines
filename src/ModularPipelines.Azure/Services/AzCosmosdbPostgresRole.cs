using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("cosmosdb", "postgres")]
public class AzCosmosdbPostgresRole
{
    public AzCosmosdbPostgresRole(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzCosmosdbPostgresRoleCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Delete(AzCosmosdbPostgresRoleDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzCosmosdbPostgresRoleDeleteOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> List(AzCosmosdbPostgresRoleListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Show(AzCosmosdbPostgresRoleShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzCosmosdbPostgresRoleShowOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Update(AzCosmosdbPostgresRoleUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzCosmosdbPostgresRoleUpdateOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Wait(AzCosmosdbPostgresRoleWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzCosmosdbPostgresRoleWaitOptions(), cancellationToken: token);
    }
}