using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dataprotection", "backup-instance")]
public class AzDataprotectionBackupInstanceRestore
{
    public AzDataprotectionBackupInstanceRestore(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> InitializeForDataRecovery(AzDataprotectionBackupInstanceRestoreInitializeForDataRecoveryOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> InitializeForDataRecoveryAsFiles(AzDataprotectionBackupInstanceRestoreInitializeForDataRecoveryAsFilesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> InitializeForItemRecovery(AzDataprotectionBackupInstanceRestoreInitializeForItemRecoveryOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Trigger(AzDataprotectionBackupInstanceRestoreTriggerOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}