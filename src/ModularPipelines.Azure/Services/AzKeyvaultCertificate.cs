using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("keyvault")]
public class AzKeyvaultCertificate
{
    public AzKeyvaultCertificate(
        AzKeyvaultCertificateContact contact,
        AzKeyvaultCertificateIssuer issuer,
        AzKeyvaultCertificatePending pending,
        ICommand internalCommand
    )
    {
        Contact = contact;
        Issuer = issuer;
        Pending = pending;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzKeyvaultCertificateContact Contact { get; }

    public AzKeyvaultCertificateIssuer Issuer { get; }

    public AzKeyvaultCertificatePending Pending { get; }

    public async Task<CommandResult> Backup(AzKeyvaultCertificateBackupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Create(AzKeyvaultCertificateCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Download(AzKeyvaultCertificateDownloadOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetDefaultPolicy(AzKeyvaultCertificateGetDefaultPolicyOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzKeyvaultCertificateGetDefaultPolicyOptions(), token);
    }

    public async Task<CommandResult> Import(AzKeyvaultCertificateImportOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzKeyvaultCertificateListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzKeyvaultCertificateListOptions(), token);
    }

    public async Task<CommandResult> ListDeleted(AzKeyvaultCertificateListDeletedOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzKeyvaultCertificateListDeletedOptions(), token);
    }

    public async Task<CommandResult> ListVersions(AzKeyvaultCertificateListVersionsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzKeyvaultCertificateListVersionsOptions(), token);
    }

    public async Task<CommandResult> Purge(AzKeyvaultCertificatePurgeOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzKeyvaultCertificatePurgeOptions(), token);
    }

    public async Task<CommandResult> Recover(AzKeyvaultCertificateRecoverOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzKeyvaultCertificateRecoverOptions(), token);
    }

    public async Task<CommandResult> Restore(AzKeyvaultCertificateRestoreOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SetAttributes(AzKeyvaultCertificateSetAttributesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzKeyvaultCertificateSetAttributesOptions(), token);
    }

    public async Task<CommandResult> Show(AzKeyvaultCertificateShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzKeyvaultCertificateShowOptions(), token);
    }

    public async Task<CommandResult> ShowDeleted(AzKeyvaultCertificateShowDeletedOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzKeyvaultCertificateShowDeletedOptions(), token);
    }
}