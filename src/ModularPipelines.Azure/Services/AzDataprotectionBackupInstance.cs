using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("dataprotection")]
public class AzDataprotectionBackupInstance
{
    public AzDataprotectionBackupInstance(
        AzDataprotectionBackupInstanceDeletedBackupInstance deletedBackupInstance,
        AzDataprotectionBackupInstanceRestore restore,
        ICommand internalCommand
    )
    {
        DeletedBackupInstance = deletedBackupInstance;
        Restore = restore;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzDataprotectionBackupInstanceDeletedBackupInstance DeletedBackupInstance { get; }

    public AzDataprotectionBackupInstanceRestore Restore { get; }

    public async Task<CommandResult> AdhocBackup(AzDataprotectionBackupInstanceAdhocBackupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Create(AzDataprotectionBackupInstanceCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzDataprotectionBackupInstanceDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDataprotectionBackupInstanceDeleteOptions(), token);
    }

    public async Task<CommandResult> Initialize(AzDataprotectionBackupInstanceInitializeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> InitializeBackupconfig(AzDataprotectionBackupInstanceInitializeBackupconfigOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> InitializeRestoreconfig(AzDataprotectionBackupInstanceInitializeRestoreconfigOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzDataprotectionBackupInstanceListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListFromResourcegraph(AzDataprotectionBackupInstanceListFromResourcegraphOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ResumeProtection(AzDataprotectionBackupInstanceResumeProtectionOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDataprotectionBackupInstanceResumeProtectionOptions(), token);
    }

    public async Task<CommandResult> Show(AzDataprotectionBackupInstanceShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDataprotectionBackupInstanceShowOptions(), token);
    }

    public async Task<CommandResult> StopProtection(AzDataprotectionBackupInstanceStopProtectionOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDataprotectionBackupInstanceStopProtectionOptions(), token);
    }

    public async Task<CommandResult> SuspendBackup(AzDataprotectionBackupInstanceSuspendBackupOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDataprotectionBackupInstanceSuspendBackupOptions(), token);
    }

    public async Task<CommandResult> Update(AzDataprotectionBackupInstanceUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDataprotectionBackupInstanceUpdateOptions(), token);
    }

    public async Task<CommandResult> UpdateMsiPermissions(AzDataprotectionBackupInstanceUpdateMsiPermissionsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdatePolicy(AzDataprotectionBackupInstanceUpdatePolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ValidateForBackup(AzDataprotectionBackupInstanceValidateForBackupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ValidateForRestore(AzDataprotectionBackupInstanceValidateForRestoreOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Wait(AzDataprotectionBackupInstanceWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDataprotectionBackupInstanceWaitOptions(), token);
    }
}