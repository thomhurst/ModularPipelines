using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("keyvault")]
public class AzKeyvaultKey
{
    public AzKeyvaultKey(
        AzKeyvaultKeyRotationPolicy rotationPolicy,
        ICommand internalCommand
    )
    {
        RotationPolicy = rotationPolicy;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzKeyvaultKeyRotationPolicy RotationPolicy { get; }

    public async Task<CommandResult> Backup(AzKeyvaultKeyBackupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Create(AzKeyvaultKeyCreateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzKeyvaultKeyCreateOptions(), token);
    }

    public async Task<CommandResult> Decrypt(AzKeyvaultKeyDecryptOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzKeyvaultKeyDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzKeyvaultKeyDeleteOptions(), token);
    }

    public async Task<CommandResult> Download(AzKeyvaultKeyDownloadOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Encrypt(AzKeyvaultKeyEncryptOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetPolicyTemplate(AzKeyvaultKeyGetPolicyTemplateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzKeyvaultKeyGetPolicyTemplateOptions(), token);
    }

    public async Task<CommandResult> Import(AzKeyvaultKeyImportOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzKeyvaultKeyImportOptions(), token);
    }

    public async Task<CommandResult> List(AzKeyvaultKeyListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzKeyvaultKeyListOptions(), token);
    }

    public async Task<CommandResult> ListDeleted(AzKeyvaultKeyListDeletedOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzKeyvaultKeyListDeletedOptions(), token);
    }

    public async Task<CommandResult> ListVersions(AzKeyvaultKeyListVersionsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzKeyvaultKeyListVersionsOptions(), token);
    }

    public async Task<CommandResult> Purge(AzKeyvaultKeyPurgeOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzKeyvaultKeyPurgeOptions(), token);
    }

    public async Task<CommandResult> Random(AzKeyvaultKeyRandomOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Recover(AzKeyvaultKeyRecoverOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzKeyvaultKeyRecoverOptions(), token);
    }

    public async Task<CommandResult> Restore(AzKeyvaultKeyRestoreOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzKeyvaultKeyRestoreOptions(), token);
    }

    public async Task<CommandResult> Rotate(AzKeyvaultKeyRotateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzKeyvaultKeyRotateOptions(), token);
    }

    public async Task<CommandResult> SetAttributes(AzKeyvaultKeySetAttributesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzKeyvaultKeySetAttributesOptions(), token);
    }

    public async Task<CommandResult> Show(AzKeyvaultKeyShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzKeyvaultKeyShowOptions(), token);
    }

    public async Task<CommandResult> ShowDeleted(AzKeyvaultKeyShowDeletedOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzKeyvaultKeyShowDeletedOptions(), token);
    }

    public async Task<CommandResult> Sign(AzKeyvaultKeySignOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Verify(AzKeyvaultKeyVerifyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}