using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("backup")]
public class AzBackupProtection
{
    public AzBackupProtection(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> AutoDisableForAzurewl(AzBackupProtectionAutoDisableForAzurewlOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AutoEnableForAzurewl(AzBackupProtectionAutoEnableForAzurewlOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> BackupNow(AzBackupProtectionBackupNowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzBackupProtectionBackupNowOptions(), token);
    }

    public async Task<CommandResult> CheckVm(AzBackupProtectionCheckVmOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzBackupProtectionCheckVmOptions(), token);
    }

    public async Task<CommandResult> Disable(AzBackupProtectionDisableOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzBackupProtectionDisableOptions(), token);
    }

    public async Task<CommandResult> EnableForAzurefileshare(AzBackupProtectionEnableForAzurefileshareOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> EnableForAzurewl(AzBackupProtectionEnableForAzurewlOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> EnableForVm(AzBackupProtectionEnableForVmOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Resume(AzBackupProtectionResumeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Undelete(AzBackupProtectionUndeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzBackupProtectionUndeleteOptions(), token);
    }

    public async Task<CommandResult> UpdateForVm(AzBackupProtectionUpdateForVmOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzBackupProtectionUpdateForVmOptions(), token);
    }
}