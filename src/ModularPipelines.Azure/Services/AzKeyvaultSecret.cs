using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("keyvault")]
public class AzKeyvaultSecret
{
    public AzKeyvaultSecret(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Backup(AzKeyvaultSecretBackupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Download(AzKeyvaultSecretDownloadOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzKeyvaultSecretListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzKeyvaultSecretListOptions(), token);
    }

    public async Task<CommandResult> ListDeleted(AzKeyvaultSecretListDeletedOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzKeyvaultSecretListDeletedOptions(), token);
    }

    public async Task<CommandResult> ListVersions(AzKeyvaultSecretListVersionsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzKeyvaultSecretListVersionsOptions(), token);
    }

    public async Task<CommandResult> Purge(AzKeyvaultSecretPurgeOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzKeyvaultSecretPurgeOptions(), token);
    }

    public async Task<CommandResult> Recover(AzKeyvaultSecretRecoverOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzKeyvaultSecretRecoverOptions(), token);
    }

    public async Task<CommandResult> Restore(AzKeyvaultSecretRestoreOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Set(AzKeyvaultSecretSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SetAttributes(AzKeyvaultSecretSetAttributesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzKeyvaultSecretSetAttributesOptions(), token);
    }

    public async Task<CommandResult> Show(AzKeyvaultSecretShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzKeyvaultSecretShowOptions(), token);
    }

    public async Task<CommandResult> ShowDeleted(AzKeyvaultSecretShowDeletedOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzKeyvaultSecretShowDeletedOptions(), token);
    }
}