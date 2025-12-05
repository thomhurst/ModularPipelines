using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("netappfiles", "account", "backup-vault")]
public class AzNetappfilesAccountBackupVaultBackup
{
    public AzNetappfilesAccountBackupVaultBackup(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzNetappfilesAccountBackupVaultBackupCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzNetappfilesAccountBackupVaultBackupDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetappfilesAccountBackupVaultBackupDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzNetappfilesAccountBackupVaultBackupListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RestoreFile(AzNetappfilesAccountBackupVaultBackupRestoreFileOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzNetappfilesAccountBackupVaultBackupShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetappfilesAccountBackupVaultBackupShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzNetappfilesAccountBackupVaultBackupUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetappfilesAccountBackupVaultBackupUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzNetappfilesAccountBackupVaultBackupWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetappfilesAccountBackupVaultBackupWaitOptions(), token);
    }
}