using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("netappfiles")]
public class AzNetappfilesAccount
{
    public AzNetappfilesAccount(
        AzNetappfilesAccountAd ad,
        AzNetappfilesAccountBackup backup,
        AzNetappfilesAccountBackupPolicy backupPolicy,
        AzNetappfilesAccountBackupVault backupVault,
        ICommand internalCommand
    )
    {
        Ad = ad;
        Backup = backup;
        BackupPolicy = backupPolicy;
        BackupVault = backupVault;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzNetappfilesAccountAd Ad { get; }

    public AzNetappfilesAccountBackup Backup { get; }

    public AzNetappfilesAccountBackupPolicy BackupPolicy { get; }

    public AzNetappfilesAccountBackupVault BackupVault { get; }

    public async Task<CommandResult> Create(AzNetappfilesAccountCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzNetappfilesAccountDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetappfilesAccountDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzNetappfilesAccountListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetappfilesAccountListOptions(), token);
    }

    public async Task<CommandResult> RenewCredentials(AzNetappfilesAccountRenewCredentialsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetappfilesAccountRenewCredentialsOptions(), token);
    }

    public async Task<CommandResult> Show(AzNetappfilesAccountShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetappfilesAccountShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzNetappfilesAccountUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetappfilesAccountUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzNetappfilesAccountWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetappfilesAccountWaitOptions(), token);
    }
}