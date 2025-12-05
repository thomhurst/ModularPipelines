using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("backup", "vault")]
public class AzBackupVaultResourceGuardMapping
{
    public AzBackupVaultResourceGuardMapping(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Delete(AzBackupVaultResourceGuardMappingDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzBackupVaultResourceGuardMappingDeleteOptions(), token);
    }

    public async Task<CommandResult> Show(AzBackupVaultResourceGuardMappingShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzBackupVaultResourceGuardMappingShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzBackupVaultResourceGuardMappingUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}