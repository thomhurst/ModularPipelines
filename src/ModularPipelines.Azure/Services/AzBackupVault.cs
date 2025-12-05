using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("backup")]
public class AzBackupVault
{
    public AzBackupVault(
        AzBackupVaultBackupProperties backupProperties,
        AzBackupVaultEncryption encryption,
        AzBackupVaultIdentity identity,
        AzBackupVaultResourceGuardMapping resourceGuardMapping,
        ICommand internalCommand
    )
    {
        BackupProperties = backupProperties;
        Encryption = encryption;
        Identity = identity;
        ResourceGuardMapping = resourceGuardMapping;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzBackupVaultBackupProperties BackupProperties { get; }

    public AzBackupVaultEncryption Encryption { get; }

    public AzBackupVaultIdentity Identity { get; }

    public AzBackupVaultResourceGuardMapping ResourceGuardMapping { get; }

    public async Task<CommandResult> Create(AzBackupVaultCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzBackupVaultDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzBackupVaultDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzBackupVaultListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzBackupVaultListOptions(), token);
    }

    public async Task<CommandResult> ListSoftDeletedContainers(AzBackupVaultListSoftDeletedContainersOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzBackupVaultShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzBackupVaultShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzBackupVaultUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzBackupVaultUpdateOptions(), token);
    }
}