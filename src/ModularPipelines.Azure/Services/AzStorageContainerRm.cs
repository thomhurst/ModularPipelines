using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("storage")]
public class AzStorageContainerRm
{
    public AzStorageContainerRm(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzStorageContainerRmCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzStorageContainerRmDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzStorageContainerRmDeleteOptions(), token);
    }

    public async Task<CommandResult> Exists(AzStorageContainerRmExistsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzStorageContainerRmExistsOptions(), token);
    }

    public async Task<CommandResult> List(AzStorageContainerRmListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> MigrateVlw(AzStorageContainerRmMigrateVlwOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzStorageContainerRmMigrateVlwOptions(), token);
    }

    public async Task<CommandResult> Show(AzStorageContainerRmShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzStorageContainerRmShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzStorageContainerRmUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzStorageContainerRmUpdateOptions(), token);
    }
}