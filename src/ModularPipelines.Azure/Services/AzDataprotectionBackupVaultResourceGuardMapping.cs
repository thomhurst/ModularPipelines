using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dataprotection", "backup-vault")]
public class AzDataprotectionBackupVaultResourceGuardMapping
{
    public AzDataprotectionBackupVaultResourceGuardMapping(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzDataprotectionBackupVaultResourceGuardMappingCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzDataprotectionBackupVaultResourceGuardMappingDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDataprotectionBackupVaultResourceGuardMappingDeleteOptions(), token);
    }

    public async Task<CommandResult> Show(AzDataprotectionBackupVaultResourceGuardMappingShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDataprotectionBackupVaultResourceGuardMappingShowOptions(), token);
    }
}