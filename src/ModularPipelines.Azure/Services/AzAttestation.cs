using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
public class AzAttestation
{
    public AzAttestation(
        AzAttestationPolicy policy,
        AzAttestationSigner signer,
        ICommand internalCommand
    )
    {
        Policy = policy;
        Signer = signer;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzAttestationPolicy Policy { get; }

    public AzAttestationSigner Signer { get; }

    public async Task<CommandResult> Create(AzAttestationCreateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAttestationCreateOptions(), token);
    }

    public async Task<CommandResult> Delete(AzAttestationDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAttestationDeleteOptions(), token);
    }

    public async Task<CommandResult> GetDefaultByLocation(AzAttestationGetDefaultByLocationOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAttestationGetDefaultByLocationOptions(), token);
    }

    public async Task<CommandResult> List(AzAttestationListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAttestationListOptions(), token);
    }

    public async Task<CommandResult> ListDefault(AzAttestationListDefaultOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAttestationListDefaultOptions(), token);
    }

    public async Task<CommandResult> Show(AzAttestationShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAttestationShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzAttestationUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAttestationUpdateOptions(), token);
    }
}