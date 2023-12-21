using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dataprotection")]
public class AzDataprotectionBackupVault
{
    public AzDataprotectionBackupVault(
        AzDataprotectionBackupVaultResourceGuardMapping resourceGuardMapping,
        ICommand internalCommand
    )
    {
        ResourceGuardMapping = resourceGuardMapping;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzDataprotectionBackupVaultResourceGuardMapping ResourceGuardMapping { get; }

    public async Task<CommandResult> Create(AzDataprotectionBackupVaultCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzDataprotectionBackupVaultDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDataprotectionBackupVaultDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzDataprotectionBackupVaultListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDataprotectionBackupVaultListOptions(), token);
    }

    public async Task<CommandResult> Show(AzDataprotectionBackupVaultShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDataprotectionBackupVaultShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzDataprotectionBackupVaultUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDataprotectionBackupVaultUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzDataprotectionBackupVaultWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDataprotectionBackupVaultWaitOptions(), token);
    }
}