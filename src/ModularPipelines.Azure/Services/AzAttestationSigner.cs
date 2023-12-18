using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("attestation")]
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
        return await _command.ExecuteCommandLineTool(options ?? new AzAttestationSignerAddOptions(), token);
    }

    public async Task<CommandResult> List(AzAttestationSignerListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAttestationSignerListOptions(), token);
    }

    public async Task<CommandResult> Remove(AzAttestationSignerRemoveOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAttestationSignerRemoveOptions(), token);
    }
}