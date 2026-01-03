using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("keyvault")]
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
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Create(AzKeyvaultCertificateCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Download(AzKeyvaultCertificateDownloadOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> GetDefaultPolicy(AzKeyvaultCertificateGetDefaultPolicyOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzKeyvaultCertificateGetDefaultPolicyOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Import(AzKeyvaultCertificateImportOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> List(AzKeyvaultCertificateListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzKeyvaultCertificateListOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> ListDeleted(AzKeyvaultCertificateListDeletedOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzKeyvaultCertificateListDeletedOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> ListVersions(AzKeyvaultCertificateListVersionsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzKeyvaultCertificateListVersionsOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Purge(AzKeyvaultCertificatePurgeOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzKeyvaultCertificatePurgeOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Recover(AzKeyvaultCertificateRecoverOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzKeyvaultCertificateRecoverOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Restore(AzKeyvaultCertificateRestoreOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> SetAttributes(AzKeyvaultCertificateSetAttributesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzKeyvaultCertificateSetAttributesOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Show(AzKeyvaultCertificateShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzKeyvaultCertificateShowOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> ShowDeleted(AzKeyvaultCertificateShowDeletedOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzKeyvaultCertificateShowDeletedOptions(), cancellationToken: token);
    }
}