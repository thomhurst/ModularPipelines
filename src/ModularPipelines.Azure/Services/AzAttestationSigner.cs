using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("attestation")]
public class AzAttestationSigner
{
    public AzAttestationSigner(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Add(AzAttestationSignerAddOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAttestationSignerAddOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> List(AzAttestationSignerListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAttestationSignerListOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Remove(AzAttestationSignerRemoveOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAttestationSignerRemoveOptions(), cancellationToken: token);
    }
}