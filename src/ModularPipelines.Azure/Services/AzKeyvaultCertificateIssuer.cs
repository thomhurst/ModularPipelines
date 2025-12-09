using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("keyvault", "certificate")]
public class AzKeyvaultCertificateIssuer
{
    public AzKeyvaultCertificateIssuer(
        AzKeyvaultCertificateIssuerAdmin admin,
        ICommand internalCommand
    )
    {
        Admin = admin;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzKeyvaultCertificateIssuerAdmin Admin { get; }

    public async Task<CommandResult> Create(AzKeyvaultCertificateIssuerCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzKeyvaultCertificateIssuerDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzKeyvaultCertificateIssuerListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzKeyvaultCertificateIssuerShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(AzKeyvaultCertificateIssuerUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}