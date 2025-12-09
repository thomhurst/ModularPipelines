using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("aks")]
public class AzAksPodIdentity
{
    public AzAksPodIdentity(
        AzAksPodIdentityException exception,
        ICommand internalCommand
    )
    {
        Exception = exception;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzAksPodIdentityException Exception { get; }

    public async Task<CommandResult> Add(AzAksPodIdentityAddOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzAksPodIdentityDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzAksPodIdentityListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}