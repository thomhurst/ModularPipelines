using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("keyvault")]
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
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Create(AzKeyvaultKeyCreateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzKeyvaultKeyCreateOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Decrypt(AzKeyvaultKeyDecryptOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Delete(AzKeyvaultKeyDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzKeyvaultKeyDeleteOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Download(AzKeyvaultKeyDownloadOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Encrypt(AzKeyvaultKeyEncryptOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> GetPolicyTemplate(AzKeyvaultKeyGetPolicyTemplateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzKeyvaultKeyGetPolicyTemplateOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Import(AzKeyvaultKeyImportOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzKeyvaultKeyImportOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> List(AzKeyvaultKeyListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzKeyvaultKeyListOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> ListDeleted(AzKeyvaultKeyListDeletedOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzKeyvaultKeyListDeletedOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> ListVersions(AzKeyvaultKeyListVersionsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzKeyvaultKeyListVersionsOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Purge(AzKeyvaultKeyPurgeOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzKeyvaultKeyPurgeOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Random(AzKeyvaultKeyRandomOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Recover(AzKeyvaultKeyRecoverOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzKeyvaultKeyRecoverOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Restore(AzKeyvaultKeyRestoreOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzKeyvaultKeyRestoreOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Rotate(AzKeyvaultKeyRotateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzKeyvaultKeyRotateOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> SetAttributes(AzKeyvaultKeySetAttributesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzKeyvaultKeySetAttributesOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Show(AzKeyvaultKeyShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzKeyvaultKeyShowOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> ShowDeleted(AzKeyvaultKeyShowDeletedOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzKeyvaultKeyShowDeletedOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Sign(AzKeyvaultKeySignOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Verify(AzKeyvaultKeyVerifyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }
}