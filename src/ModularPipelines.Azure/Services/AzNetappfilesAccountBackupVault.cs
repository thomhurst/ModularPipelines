using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("netappfiles", "account")]
public class AzNetappfilesAccountBackupVault
{
    public AzNetappfilesAccountBackupVault(
        AzNetappfilesAccountBackupVaultBackup backup,
        ICommand internalCommand
    )
    {
        Backup = backup;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzNetappfilesAccountBackupVaultBackup Backup { get; }

    public async Task<CommandResult> Create(AzNetappfilesAccountBackupVaultCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzNetappfilesAccountBackupVaultDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzNetappfilesAccountBackupVaultListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzNetappfilesAccountBackupVaultShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetappfilesAccountBackupVaultShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzNetappfilesAccountBackupVaultUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetappfilesAccountBackupVaultUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzNetappfilesAccountBackupVaultWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetappfilesAccountBackupVaultWaitOptions(), token);
    }
}